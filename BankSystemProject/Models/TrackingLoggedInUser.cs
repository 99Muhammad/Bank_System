

namespace BankSystemProject.Model
{
    public class TrackingLoggedInUser
    {
        //  public int LoggedInId { get; set; }  // Primary Key
        //  public DateTime LoginTime { get; set; }
        //  public DateTime? LogoutTime { get; set; }

        ////  public bool IsActive { get; set; }
        //  public int CustomerAccountID { get; set; }         // Foreign Key to Users
        //  public CustomerAccount customerAccount { get; set; }          // Navigation property

        public int LoggedInId { get; set; } // Primary Key
        public DateTime LoginTime { get; set; }
       // public DateTime? LogoutTime { get; set; }

        //public string UserType { get; set; }

        public string UserID { get; set; }//FK

        public Users users { get; set; }
       // public int? CustomerAccountID { get; set; } // Foreign Key to Customers (nullable for Employees)
        //public CustomerAccount CustomerAccount { get; set; } // Navigation property for Customers

        //public int? EmployeeAccountID { get; set; } // Foreign Key to Employees (nullable for Customers)
        //public Employee EmployeeAccount { get; set; } // Navigation property for Employees
    }

}
