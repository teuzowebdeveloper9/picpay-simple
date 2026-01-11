using SimplePicPay.Models.Enums;

namespace SimplePicPay.Models;

public class WalletEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CPFCNPJ { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public UserType UserType { get; set; }


    public WalletEntity(Guid id, string name, string cPFCNPJ, string email, string passwordHash, decimal balance = 0, UserType userType = UserType.user)
    {
        Id = id;
        Name = name;
        CPFCNPJ = cPFCNPJ;
        Email = email;
        PasswordHash = passwordHash;
        Balance = balance;
        UserType = userType;
    }

    public void AddBalance(decimal amount)
    {
        Balance += amount;
    }

    public void WithdrawBalance(decimal amount)
    {
        Balance -= amount;
    }
}