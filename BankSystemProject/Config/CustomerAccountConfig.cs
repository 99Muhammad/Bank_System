using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class CustomerAccountConfig : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.HasKey(ca => ca.CustomerAccountId);

            builder.HasOne(ca => ca.User)
                   .WithMany(u => u.CustomerAccounts)
                   .HasForeignKey(ca => ca.UserId);

            builder.HasOne(ca => ca.AccountType)
                   .WithMany(at => at.CustomerAccounts)
                   .HasForeignKey(ca => ca.AccountTypeId);

            builder.HasMany(t => t.transactions)
                .WithOne(t => t.customerAccount)
                .HasForeignKey(c => c.TransactionId);

            //builder.Property(ca => ca.AccountNumber)
            //       .HasMaxLength(50)
            //       .IsRequired();

            //builder.Property(ca => ca.PinCode)
            //       .HasMaxLength(6)
            //       .IsRequired();

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime"); // Ensure EF Core uses SQL Server's DateTime type

            //builder.Property(ca => ca.Balance)
            //       .HasColumnType("decimal(18,2)")
            //       .IsRequired();
        }
    }

}
