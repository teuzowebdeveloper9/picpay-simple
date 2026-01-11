namespace SimplePicPay.Infra;


using Microsoft.EntityFrameworkCore;
using SimplePicPay.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<WalletEntity> Wallets { get; set; }
    public DbSet<TranferEntity> tranfers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}