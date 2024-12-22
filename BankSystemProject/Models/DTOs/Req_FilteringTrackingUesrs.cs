namespace BankSystemProject.Models.DTOs
{
    public class Req_FilteringTrackingUesrs
    {
        public string Role { get; set; } // Filter by role (e.g., Admin, User, etc.)
        //public DateTime LoginAfter { get; set; } // Filter users logged in after this time
        public string SearchTerm { get; set; } // Search by username or email
    }
}
