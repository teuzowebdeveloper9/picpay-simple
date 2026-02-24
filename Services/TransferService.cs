using Microsoft.EntityFrameworkCore;
using SimplePicPay.DTOs;
using SimplePicPay.Infra;
using SimplePicPay.Models;
using SimplePicPay.Models.Enums;

namespace SimplePicPay.Services;

public class TransferService : ITransferService
{
    private readonly AppDbContext _context;

    public TransferService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TransferResponse> CreateTransferAsync(CreateTransferRequest request)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("O valor da transferência deve ser positivo.");

        if (request.SenderId == request.ReceiverId)
            throw new ArgumentException("O remetente e destinatário não podem ser iguais.");

        var sender = await _context.Wallets.FindAsync(request.SenderId);
        if (sender is null)
            throw new KeyNotFoundException("Remetente não encontrado.");

        var receiver = await _context.Wallets.FindAsync(request.ReceiverId);
        if (receiver is null)
            throw new KeyNotFoundException("Destinatário não encontrado.");

        // Regra: Lojistas não podem enviar transferências
        if (sender.UserType == UserType.merchant)
            throw new InvalidOperationException("Lojistas não podem enviar transferências, apenas receber.");

        // Regra: Verificar saldo suficiente
        if (sender.Balance < request.Amount)
            throw new InvalidOperationException("Saldo insuficiente para realizar a transferência.");

        // Executar transferência
        sender.WithdrawBalance(request.Amount);
        receiver.AddBalance(request.Amount);

        var transfer = new TransferEntity(
            transferId: Guid.NewGuid(),
            senderId: request.SenderId,
            receiverId: request.ReceiverId,
            amount: request.Amount
        );

        _context.Transfers.Add(transfer);
        await _context.SaveChangesAsync();

        return MapToResponse(transfer);
    }

    public async Task<TransferResponse?> GetTransferByIdAsync(Guid id)
    {
        var transfer = await _context.Transfers.FindAsync(id);
        return transfer is null ? null : MapToResponse(transfer);
    }

    public async Task<IEnumerable<TransferResponse>> GetTransfersByWalletIdAsync(Guid walletId)
    {
        var transfers = await _context.Transfers
            .Where(t => t.SenderId == walletId || t.ReceiverId == walletId)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();

        return transfers.Select(MapToResponse);
    }

    public async Task<IEnumerable<TransferResponse>> GetAllTransfersAsync()
    {
        var transfers = await _context.Transfers
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();

        return transfers.Select(MapToResponse);
    }

    private static TransferResponse MapToResponse(TransferEntity transfer)
    {
        return new TransferResponse(
            transfer.TransferId,
            transfer.SenderId,
            transfer.ReceiverId,
            transfer.Amount,
            transfer.TransferDate
        );
    }
}

