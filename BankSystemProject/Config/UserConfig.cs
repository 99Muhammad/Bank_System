using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class UserConfig : IEntityTypeConfiguration<Users>
    {
      
        public void Configure(EntityTypeBuilder<Users> builder)
        {
           
                builder.HasKey(u => u.Id);

                builder.Property(u => u.FullName)
                       .HasMaxLength(200)
                       .IsRequired();

                builder.Property(u => u.Email)
                       .HasMaxLength(100)
                       .IsRequired();

                builder.Property(u => u.PhoneNumber)
                       .HasMaxLength(20);

                builder.Property(u => u.Address)
                       .HasMaxLength(250);

                builder.Property(u => u.DateOfBirth)
                       .IsRequired();

                builder.Property(u => u.Role)
                       .IsRequired();

                builder.Property(u => u.PersonalImage)
                       .HasMaxLength(500);

                builder.Property(u => u.Gender)
                       .IsRequired();

                builder.HasMany(u => u.CustomerAccounts)
                       .WithOne(ca => ca.User)
                       .HasForeignKey(ca => ca.UserId);

            //builder.HasMany(u=>u.TrackingLoggedInUsers)
            //    .WithOne(ca => ca.User)
            //    .HasForeignKey(ca => ca.UserId);


            
        }
    }
}
