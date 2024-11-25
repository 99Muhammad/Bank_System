using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class LoanConfig : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.LoanId);

            builder.HasOne(l => l.CustomerAccount)
                   .WithMany(ca => ca.Loans)
                   .HasForeignKey(l => l.CustomerAccountId);

            builder.HasOne(l => l.LoanType)
                   .WithMany(lt => lt.Loans)
                   .HasForeignKey(l => l.LoanTypeId);

            builder.Property(l => l.LoanAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(l => l.StartDate)
                   .IsRequired();
        }
    }

}
