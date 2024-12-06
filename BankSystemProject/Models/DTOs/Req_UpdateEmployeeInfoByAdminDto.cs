using BankSystemProject.Shared.Enums;
using BankSystemProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_UpdateEmployeeInfoByAdminDto
    {
  
        [Required(ErrorMessage = "User role is required.")]
        public enUserRole UserRole { get; set; }

        // Conditionally show Salary field for employees only
        //[SalaryValidation(ErrorMessage = "Salary is required for employees.")]
        [Required]
        public int? Salary { get; set; }
        public enBranches BrnachName { get; set; }
        //public string  BranchName { get; set; }
        //public string BranchLocation { get; set; }
        public bool IsDeleted { get; set; }
    }
}
