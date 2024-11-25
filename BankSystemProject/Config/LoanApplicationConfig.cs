using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class LoanApplicationConfig : IEntityTypeConfiguration<LoanApplication>
    {
        public void Configure(EntityTypeBuilder<LoanApplication> builder)
        {
            builder.HasKey(la => la.LoanApplicationId);

            builder.HasOne(la => la.LoanType)
                   .WithMany(l => l.LoanApplications)
                   .HasForeignKey(la => la.LoanTypeId);

            builder.HasOne(la => la.CustomerAccount)
                   .WithMany(ca => ca.LoanApplications)
                   .HasForeignKey(la => la.CustomerAccountId);

            builder.Property(la => la.ApplicationDate)
                   .IsRequired();

            builder.Property(la => la.Status)
                   .IsRequired();
        }
    }

}
