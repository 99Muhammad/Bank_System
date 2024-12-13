using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class TrackingLoggedInUserConfig : IEntityTypeConfiguration<TrackingLoggedInUser>
    {
        public void Configure(EntityTypeBuilder<TrackingLoggedInUser> builder)
        {
            builder.HasKey(tlu => tlu.LoggedInId);

            builder.HasOne(tlu => tlu.users)
                   .WithMany(u => u.TrackingLoggedInUsers)
                   .HasForeignKey(tlu => tlu.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(tlu => tlu.EmployeeAccount)
            //      .WithMany(u => u.trackingLoggedInUsers)
            //      .HasForeignKey(tlu => tlu.CustomerAccountID)
            //      .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
