namespace ExpenseTracker.Domain.Common;

public static class Constants
{
    public static string TransactionType { get; } = "TRANSACTION_TYPE";
    public static string TransactionCategory { get; } = "TRANSACTION_CATEGORY";
    public static string WalletImageTypeCode { get; set; } = "WALLET";
    public static string TransactionReceiptImageTypeCode { get; set; } = "RECEIPT";
}