using SimplePicPay.Models.Enums;

namespace SimplePicPay.DTOs;

public record CreateWalletRequest(
    string Name,
    string CPFCNPJ,
    string Email,
    string Password,
    UserType UserType = UserType.user
);

public record WalletResponse(
    Guid Id,
    string Name,
    string CPFCNPJ,
    string Email,
    decimal Balance,
    UserType UserType
);

public record DepositRequest(decimal Amount);

public record WalletBalanceResponse(Guid Id, string Name, decimal Balance);

