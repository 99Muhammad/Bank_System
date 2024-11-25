using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(cc => cc.CreditCardId);

            builder.HasOne(cc => cc.CustomerAccount)
                   .WithMany(ca => ca.CreditCards)
                   .HasForeignKey(cc => cc.CustomerAccountId);

            //builder.Property(cc => cc.CreditLimit)
            //       .HasColumnType("decimal(18,2)");

            //builder.Property(cc => cc.ExpiryDate)
            //       .IsRequired();
        }
    }

}
