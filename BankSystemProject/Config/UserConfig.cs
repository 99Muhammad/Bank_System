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

            //builder.Property(u => u.FullName)
            //       .HasMaxLength(200);


            //builder.Property(u => u.Email)
            //       .HasMaxLength(100);
                       

            //    builder.Property(u => u.PhoneNumber)
            //           .HasMaxLength(20);

            //    builder.Property(u => u.Address)
            //           .HasMaxLength(250);

            //builder.Property(u => u.DateOfBirth)
            //       ;

            //builder.Property(u => u.Role)
            //      ;

            //builder.Property(u => u.PersonalImage)
            //      ;

            //builder.Property(u => u.Gender)
            //       ;

                builder.HasMany(u => u.CustomerAccounts)
                       .WithOne(ca => ca.User)
                       .HasForeignKey(ca => ca.UserId);

            builder.Property(e => e.DateOfBirth)
              .HasColumnType("datetime");

            //builder.HasMany(u=>u.TrackingLoggedInUsers)
            //    .WithOne(ca => ca.customerAccount)
            //    .HasForeignKey(ca => ca.CustomerAccountID);



        }
    }
}
