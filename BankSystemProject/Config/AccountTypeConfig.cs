using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class AccountTypeConfig : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            builder.HasKey(at => at.AccountTypeId);

            builder.Property(at => at.AccountTypeName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(at => at.Description)
                   .HasMaxLength(250);
        }
    }

}
