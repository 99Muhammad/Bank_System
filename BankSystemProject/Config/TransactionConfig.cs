using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.HasOne(t => t.CustomerAccount)
                   .WithMany(ca => ca.Transactions)
                   .HasForeignKey(t => t.CustomerAccountId);

            //builder.Property(t => t.Amount)
            //       .HasColumnType("decimal(18,2)");

            //builder.Property(t => t.TransactionDate)
            //       .IsRequired();
        }
    }
}
