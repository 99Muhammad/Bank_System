using Microsoft.AspNetCore.Identity;

namespace BankSystemProject.Model
{
    public class Users:IdentityUser
    {
        public string FullName { get; set; }
        //public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
        public string PersonalImage { get; set; }
        public bool IsDeleted { get; set; }
        public string Gender { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; }
       // public ICollection<Branch> ManagedBranches { get; set; }
        
        //public ICollection<TransferInfo> TransferInfos { get; set; }
       // public ICollection<TrackingLoggedInUser> TrackingLoggedInUsers { get; set; }
        public ICollection<Employee> employees { get; set; }
    }

}
