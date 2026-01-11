using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplePicPay.Models;

namespace SimplePicPay.Infra.Config;

public class WalletConfig : IEntityTypeConfiguration<WalletEntity> 
{
    public void Configure(EntityTypeBuilder<WalletEntity> builder)
    {
        builder.ToTable("wallets");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name)
        .IsRequired()
        .HasMaxLength(200);
        builder.Property(w => w.CPFCNPJ)
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(w => w.CPFCNPJ)
            .IsUnique();
        builder.Property(w => w.Email)
            .IsRequired()
            .HasMaxLength(200);
        builder.HasIndex(w => w.Email)
            .IsUnique();
        builder.Property(w => w.Balance).HasPrecision(18, 2);
        builder.Property(w => w.UserType)
       .HasConversion<int>()
       .IsRequired();

       
    }
}