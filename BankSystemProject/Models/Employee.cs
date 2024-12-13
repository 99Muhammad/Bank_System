namespace BankSystemProject.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; } 
        public string UserId { get; set; }   
        public DateTime HireDate { get; set; }
        public int? EmployeeSalary { get; set; }
        public bool IsDeleted { get; set; }
        public Users User { get; set; }
        public int BranchID { get; set; }
        public Branch BranchEmployee { get; set; }

     
    }
}
