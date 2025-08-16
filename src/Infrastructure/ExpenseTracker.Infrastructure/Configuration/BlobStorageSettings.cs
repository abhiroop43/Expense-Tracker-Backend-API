namespace ExpenseTracker.Infrastructure.Configuration;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = default!;
    public string WalletContainerName { get; set; } = default!;
    public string TransactionReceiptContainerName { get; set; } = default!;
    public string AccountKey { get; set; } = default!;
}