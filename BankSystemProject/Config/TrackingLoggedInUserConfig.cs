using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BankSystemProject.Model;

namespace BankSystemProject.Config
{
    public class TrackingLoggedInUserConfig : IEntityTypeConfiguration<TrackingLoggedInUser>
    {
        public void Configure(EntityTypeBuilder<TrackingLoggedInUser> builder)
        {
            builder.HasKey(tlu => tlu.LoggedInUserId);

            builder.HasOne(tlu => tlu.User)
                   .WithMany(u => u.TrackingLoggedInUsers)
                   .HasForeignKey(tlu => tlu.UserId);

            builder.Property(tlu => tlu.LoginTime)
                   .IsRequired();

            builder.Property(tlu => tlu.LogoutTime);

            //builder.Property(tlu => tlu.IsActive)
            //       .IsRequired();
        }
    }

}
