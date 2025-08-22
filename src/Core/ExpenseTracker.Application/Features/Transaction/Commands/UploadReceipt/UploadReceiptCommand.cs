using MediatR;

namespace ExpenseTracker.Application.Features.Transaction.Commands.UploadReceipt;

public class UploadReceiptCommand : IRequest<string>
{
    public string FileName { get; set; } = default!;
    public Stream File { get; set; } = default!;
}