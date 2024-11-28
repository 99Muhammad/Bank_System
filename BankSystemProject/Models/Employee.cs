namespace BankSystemProject.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; } 
        public string UserId { get; set; }   
        public DateTime HireDate { get; set; }
        public int EmployeeSalary { get; set; }
        public Users User { get; set; }  
        public ICollection<Branch> ManagedBranches { get; set; }
    }

}
