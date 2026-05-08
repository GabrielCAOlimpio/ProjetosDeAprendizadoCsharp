

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinTrack.Domain.Entities;

namespace  FinTrack.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.Type).IsRequired().HasDefaultValue(0);
            builder.Property(c => c.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(c => c.Date).IsRequired();

            builder.HasIndex(t => t.Date);

            builder.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}