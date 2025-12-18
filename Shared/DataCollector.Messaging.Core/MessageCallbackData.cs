namespace DataCollector.Messaging.Core;

public readonly record struct MessageCallbackData(byte[] Payload, string Route);