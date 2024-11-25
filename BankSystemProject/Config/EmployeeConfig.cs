using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(u => u.EmployeeId);

            builder.HasOne(u => u.User)
              .WithMany(u => u.employees)
              .HasForeignKey(u => u.UserId);
        }
    }

        }
