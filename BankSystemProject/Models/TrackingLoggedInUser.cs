

namespace BankSystemProject.Model
{
    public class TrackingLoggedInUser
    {
        public int LoggedInId { get; set; } 
        public DateTime LoginTime { get; set; }
      
        public string UserID { get; set; }//FK

        public Users users { get; set; }
      
    }

}
