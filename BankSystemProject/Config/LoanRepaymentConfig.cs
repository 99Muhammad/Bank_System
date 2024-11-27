using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class LoanRepaymentConfig : IEntityTypeConfiguration<LoanRepayment>
    {
        public void Configure(EntityTypeBuilder<LoanRepayment> builder)
        {
            builder.HasKey(lr => lr.LoanRepaymentId);

            builder.HasOne(lr => lr.Loan)
                   .WithMany(l => l.LoanRepayments)
                   .HasForeignKey(lr => lr.LoanId);

            //builder.Property(lr => lr.AmountPaid)
            //       .HasColumnType("decimal(18,2)");

            builder.Property(lr => lr.PaymentDate)
                   .IsRequired();
        }
    }

}
