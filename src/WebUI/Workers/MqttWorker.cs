using Application.Common.Models;
using Application.Extensions;
using Application.WeatherForecasts.Commands;
using Infrastructure.Configurations;
using MediatR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System.Text.Json;

namespace Infrastructure.Workers
{
    public class MQTTWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MQTTWorker> _logger;
        private readonly MQTTOptions _mqttOptions;
        private readonly Queue<VehiclePositionPayload> _queue;

        public MQTTWorker(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<MQTTWorker> logger,
            MQTTOptions mqttOptions)
        {
            this._serviceScopeFactory = serviceScopeFactory;
            this._logger = logger;
            this._mqttOptions = mqttOptions;
            _queue = new Queue<VehiclePositionPayload>();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Subscribe(stoppingToken);
        }

        public async Task Subscribe(CancellationToken stoppingToken)
        {
            var _factory = new MqttFactory();
            var _mqttClient = _factory.CreateMqttClient();

            // Use TCP connection.
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_mqttOptions.Host, _mqttOptions.Port)
                .WithTls()
                .WithCleanSession()
                .Build();

            // Connect
            await _mqttClient.ConnectAsync(options, stoppingToken);

            // Subscribe to topic
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_mqttOptions.BusTopic).Build());

            // Set up handlers for connected & disconnected scenarios
            _mqttClient.UseConnectedHandler(OnConnected);
            _mqttClient.UseDisconnectedHandler(async e => await OnDisconnected(e, _mqttClient, options));

            // Setup handler for handling reciving messages
            _mqttClient.UseApplicationMessageReceivedHandler(OnMessageRecieved);
        }

        #region Handlers
        public async Task OnMessageRecieved(MqttApplicationMessageReceivedEventArgs message)
        {
            string stringMessagePayload = System.Text.Encoding.UTF8.GetString(message.ApplicationMessage.Payload);

            _logger.LogInformation("### RECEIVED APPLICATION MESSAGE ###");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                TopicPayload? messagePayload = JsonSerializer.Deserialize<TopicPayload>(stringMessagePayload);

                // Queee message
                if (messagePayload != null)
                    _queue.Enqueue(messagePayload.VP);
                else
                    return;

                // Quee Size
                var queueSize = 0;
                _queue.TryGetNonEnumeratedCount(out queueSize);
                _logger.LogInformation($"### QUEUE SIZE {queueSize} ###");

                // Deque batch of 100 items
                if (queueSize >= 100)
                {
                    // Dequee
                    var messages = _queue.DequeueChunk<VehiclePositionPayload>(queueSize);
                    var vehiclePositions = messages.ToList();
                    _logger.LogInformation($"### DEQUED  {vehiclePositions.Count()} MESSAGES ###");

                    // Persist
                    await _mediator.Send(new CreateBatchVehiclePositionCommand() { VehiclePositions = vehiclePositions });
                }
            };
        }

        public async Task OnDisconnected(MqttClientDisconnectedEventArgs obj, IMqttClient _mqttClient, IMqttClientOptions options)
        {
            _logger.LogInformation("### DISCONNECTED FROM SERVER ###");
            await Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                // Connect
                await _mqttClient.ConnectAsync(options, CancellationToken.None);

                // Subscribe to topic
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_mqttOptions.BusTopic).Build());
            }
            catch
            {
                _logger.LogInformation("### RECONNECTING FAILED ###");
            }
        }

        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _logger.LogInformation("Successfully connected.");
        }
        #endregion
    }
}