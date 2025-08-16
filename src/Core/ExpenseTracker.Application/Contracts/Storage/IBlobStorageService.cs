namespace ExpenseTracker.Application.Contracts.Storage;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(Stream data, string fileName, string imageType);
    string? GetBlobSasUrl(string? blobUrl, string imageType);
}