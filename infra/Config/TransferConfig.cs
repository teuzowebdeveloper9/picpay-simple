using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplePicPay.Models;

namespace SimplePicPay.Infra.Config;

public class TransferConfig : IEntityTypeConfiguration<TransferEntity>
{
    public void Configure(EntityTypeBuilder<TransferEntity> builder)
    {
        builder.ToTable("transfers");
        builder.HasKey(t => t.TransferId);
        builder.Property(t => t.Amount)
       .HasPrecision(18, 2)
       .IsRequired();
       builder.HasOne<WalletEntity>()
       .WithMany()
       .HasForeignKey(t => t.SenderId)
       .OnDelete(DeleteBehavior.Restrict);
       builder.HasOne<WalletEntity>()
       .WithMany()
       .HasForeignKey(t => t.ReceiverId)
       .OnDelete(DeleteBehavior.Restrict);
        
    }
}