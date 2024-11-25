using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class TransferInfoConfig : IEntityTypeConfiguration<TransferInfo>
    {
        public void Configure(EntityTypeBuilder<TransferInfo> builder)
        {
            builder.HasKey(t => t.TransferInfoId);

            builder.HasOne(t => t.User)
                   .WithMany(ca => ca.TransferInfos)
                   .HasForeignKey(t => t.UserId);

            
        }
    }
}
