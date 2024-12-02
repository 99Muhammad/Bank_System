using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class TransactionConfig : IEntityTypeConfiguration<TransactionsDepWi>
    {
        public void Configure(EntityTypeBuilder<TransactionsDepWi> builder)
        {
            builder.HasKey(t => t.TransactionId);

            //builder.HasOne(t => t.customerAccount)
            //       .WithMany(ca =>ca.transactions)
            //       .HasForeignKey(t => t.CustomerAccountId);

            //builder.Property(t => t.Amount)
            //       .HasColumnType("decimal(18,2)");

            //builder.Property(t => t.TransactionDate)
            //       .IsRequired();
        }
    }
}
