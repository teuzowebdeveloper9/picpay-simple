using SimplePicPay.DTOs;

namespace SimplePicPay.Services;

public interface ITransferService
{
    Task<TransferResponse> CreateTransferAsync(CreateTransferRequest request);
    Task<TransferResponse?> GetTransferByIdAsync(Guid id);
    Task<IEnumerable<TransferResponse>> GetTransfersByWalletIdAsync(Guid walletId);
    Task<IEnumerable<TransferResponse>> GetAllTransfersAsync();
}

