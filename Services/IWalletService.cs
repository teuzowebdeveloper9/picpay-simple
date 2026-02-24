using SimplePicPay.DTOs;
using SimplePicPay.Models;

namespace SimplePicPay.Services;

public interface IWalletService
{
    Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request);
    Task<WalletResponse?> GetWalletByIdAsync(Guid id);
    Task<IEnumerable<WalletResponse>> GetAllWalletsAsync();
    Task<WalletBalanceResponse> DepositAsync(Guid walletId, decimal amount);
    Task<bool> DeleteWalletAsync(Guid id);
}

