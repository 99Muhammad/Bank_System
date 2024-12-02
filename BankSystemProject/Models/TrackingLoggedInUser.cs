namespace BankSystemProject.Model
{
    public class TrackingLoggedInUser
    {
        public int LoggedInUserId { get; set; }  // Primary Key
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

      //  public bool IsActive { get; set; }
        public int CustomerAccountID { get; set; }         // Foreign Key to Users
        public CustomerAccount customerAccount { get; set; }          // Navigation property
    }

}
