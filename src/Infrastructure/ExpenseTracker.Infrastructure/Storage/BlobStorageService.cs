using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettings.Value;

    public async Task<string> UploadToBlobAsync(Stream data, string fileName, string imageType)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        BlobContainerClient containerClient;

        if (imageType == Constants.WalletImageTypeCode)
            containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.WalletContainerName);
        else if (imageType == Constants.TransactionReceiptImageTypeCode)
            containerClient =
                blobServiceClient.GetBlobContainerClient(_blobStorageSettings.TransactionReceiptContainerName);
        else
            throw new BadRequestException("Invalid image type code");

        var blobClient = containerClient.GetBlobClient(fileName);

        if (data.CanSeek) data.Position = 0;

        await blobClient.UploadAsync(data, true);
        var url = blobClient.Uri.ToString();
        return GetBlobSasUrl(url, imageType)!;
    }

    public string? GetBlobSasUrl(string? blobUrl, string imageType)
    {
        if (blobUrl == null) return null;

        string containerName;
        if (imageType == Constants.WalletImageTypeCode)
            containerName = _blobStorageSettings.WalletContainerName;
        else if (imageType == Constants.TransactionReceiptImageTypeCode)
            containerName =
                _blobStorageSettings.TransactionReceiptContainerName;
        else
            return null;

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };

        sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

        var sasToken = sasBuilder
            .ToSasQueryParameters(new StorageSharedKeyCredential(blobServiceClient.AccountName,
                _blobStorageSettings.AccountKey)).ToString();

        return $"{blobUrl}?{sasToken}";
    }

    private static string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments[^1];
    }
}