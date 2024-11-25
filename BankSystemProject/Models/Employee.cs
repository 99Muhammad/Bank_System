namespace BankSystemProject.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }  // Primary Key
        public string UserId { get; set; }      // Foreign Key to Users
        public DateTime HireDate { get; set; }

        public Users User { get; set; }       // Navigation property to Users
        public ICollection<Branch> ManagedBranches { get; set; } // Employees may manage branches
    }

}
