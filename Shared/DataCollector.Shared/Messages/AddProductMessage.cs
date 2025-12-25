namespace DataCollector.Shared.Messages;

public record AddProductMessage(string Barcode, string Name, string Description);