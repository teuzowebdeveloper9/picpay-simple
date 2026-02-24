using Microsoft.EntityFrameworkCore;
using SimplePicPay.DTOs;
using SimplePicPay.Infra;
using SimplePicPay.Models;

namespace SimplePicPay.Services;

public class WalletService : IWalletService
{
    private readonly AppDbContext _context;

    public WalletService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WalletResponse> CreateWalletAsync(CreateWalletRequest request)
    {
        var existingEmail = await _context.Wallets.AnyAsync(w => w.Email == request.Email);
        if (existingEmail)
            throw new InvalidOperationException("Email já cadastrado no sistema.");

        var existingCpfCnpj = await _context.Wallets.AnyAsync(w => w.CPFCNPJ == request.CPFCNPJ);
        if (existingCpfCnpj)
            throw new InvalidOperationException("CPF/CNPJ já cadastrado no sistema.");

        var wallet = new WalletEntity(
            id: Guid.NewGuid(),
            name: request.Name,
            cPFCNPJ: request.CPFCNPJ,
            email: request.Email,
            passwordHash: HashPassword(request.Password),
            balance: 0,
            userType: request.UserType
        );

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return MapToResponse(wallet);
    }

    public async Task<WalletResponse?> GetWalletByIdAsync(Guid id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        return wallet is null ? null : MapToResponse(wallet);
    }

    public async Task<IEnumerable<WalletResponse>> GetAllWalletsAsync()
    {
        var wallets = await _context.Wallets.ToListAsync();
        return wallets.Select(MapToResponse);
    }

    public async Task<WalletBalanceResponse> DepositAsync(Guid walletId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor do depósito deve ser positivo.");

        var wallet = await _context.Wallets.FindAsync(walletId);
        if (wallet is null)
            throw new KeyNotFoundException("Carteira não encontrada.");

        wallet.AddBalance(amount);
        await _context.SaveChangesAsync();

        return new WalletBalanceResponse(wallet.Id, wallet.Name, wallet.Balance);
    }

    public async Task<bool> DeleteWalletAsync(Guid id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet is null)
            return false;

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();
        return true;
    }

    private static WalletResponse MapToResponse(WalletEntity wallet)
    {
        return new WalletResponse(
            wallet.Id,
            wallet.Name,
            wallet.CPFCNPJ,
            wallet.Email,
            wallet.Balance,
            wallet.UserType
        );
    }

    private static string HashPassword(string password)
    {
        // Em produção, usar BCrypt ou similar
        return Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password)
            )
        );
    }
}

