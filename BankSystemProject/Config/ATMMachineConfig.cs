using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class ATMMachineConfig : IEntityTypeConfiguration<ATMMachine>
    {
        public void Configure(EntityTypeBuilder<ATMMachine> builder)
        {
            builder.HasKey(atm => atm.ATMMachineId);

            builder.HasOne(atm => atm.Branch)
                   .WithMany(b => b.ATMMachines)
                   .HasForeignKey(atm => atm.BranchId);

            //builder.Property(atm => atm.Location)
            //       .HasMaxLength(100)
            //       .IsRequired();
        }
    }

}
