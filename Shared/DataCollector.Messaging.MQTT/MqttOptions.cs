namespace DataCollector.Messaging.MQTT;

public class MqttOptions
{
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 1883;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
