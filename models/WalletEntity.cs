using SimplePicPay.Models.Enums;

namespace SimplePicPay.Models;

public class WalletEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CPFCNPJ { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public UserType UserType { get; set; }
}