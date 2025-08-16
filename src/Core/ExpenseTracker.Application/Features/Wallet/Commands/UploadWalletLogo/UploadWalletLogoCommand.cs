using MediatR;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UploadWalletLogo;

public class UploadWalletLogoCommand : IRequest<string>
{
    public string FileName { get; set; } = default!;
    public Stream File { get; set; } = default!;
}