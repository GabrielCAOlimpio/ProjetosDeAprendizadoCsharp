using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Data.Configurations;

public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.ToTable("Balance");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("SYSDATETIME()");

        builder.HasOne(b => b.User)
            .WithOne(u => u.Balance)
            .HasForeignKey<Balance>(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(b => b.UserId)
            .IsUnique();
        
    }
}