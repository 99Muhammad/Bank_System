using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class BankFeeConfig : IEntityTypeConfiguration<BankFee>
    {
        public void Configure(EntityTypeBuilder<BankFee> builder)
        {
            builder.HasKey(bf => bf.BankFeeId);

            //builder.Property(bf => bf.FeeType)
            //       .HasMaxLength(100)
            //       .IsRequired();

            //builder.Property(bf => bf.Amount)
            //       .HasColumnType("decimal(18,2)")
            //       .IsRequired();
        }
    }

}
