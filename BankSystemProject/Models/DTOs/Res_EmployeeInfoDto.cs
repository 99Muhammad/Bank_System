namespace BankSystemProject.Models.DTOs
{
    public class Res_EmployeeInfoDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string PersonalImage { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }

    }
}
