namespace Infrastructure.Configurations
{
    public class MQTTOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string BusTopic { get; set; }
        public int QueueSize { get; set; }
    }
}