using Application.WeatherForecasts.Commands;
using MediatR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace Infrastructure.Workers
{
    public class MqttWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MqttWorker(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Subscribe(stoppingToken);
        }

        public async Task Subscribe(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();


            // Use TCP connection.
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.hsl.fi", 8883)
                .WithTls()
                .WithCleanSession()
                .Build();

            // connect
            await mqttClient.ConnectAsync(options, CancellationToken.None);

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("/hfp/v2/journey/ongoing/vp/bus/+/+/+/+/+/+/#").Build());

            mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                string result = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                Console.WriteLine($"MQTT Client: OnNewMessage Topic: {e.ApplicationMessage.Topic} / Message: {result}");
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");

                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await _mediator.Send(new CreateRouteCommand() { Title = "Result" });

                        await Task.Delay(TimeSpan.FromMilliseconds(30));
                    };
                }
            });
        }
    }
}