namespace DataCollector.Messaging.AMQP;

public class RabbitMQOptions
{
    public string Host { get; set; } = "rabbitmq";

    public int Port { get; set; } = 5672;

    public string VirtualHost { get; set; } = "/";

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
