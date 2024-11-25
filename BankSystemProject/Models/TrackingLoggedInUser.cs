namespace BankSystemProject.Model
{
    public class TrackingLoggedInUser
    {
        public int LoggedInUserId { get; set; }  // Primary Key
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }         // Foreign Key to Users
        public Users User { get; set; }          // Navigation property
    }

}
