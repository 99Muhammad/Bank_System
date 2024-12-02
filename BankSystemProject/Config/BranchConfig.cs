using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(b => b.BranchId);

            builder.HasMany(u => u.employees)
                .WithOne(u => u.BranchEmployee)
                .HasForeignKey(u => u.BranchID);


            //builder.Property(b => b.BranchName)
            //       .HasMaxLength(200)
            //       .IsRequired();

            //builder.Property(b => b.BranchLocation)
            //       .HasMaxLength(250)
            //       .IsRequired();
        }
    }

}
