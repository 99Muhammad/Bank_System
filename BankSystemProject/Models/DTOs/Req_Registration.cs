using BankSystemProject.Validation;
using BankSystemProject.Shared.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BankSystemProject.Models.DTOs
{

    public class Req_Registration
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second name is required.")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Third name is required.")]
        public string ThirdName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "It should contain of 10 numbers")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long.")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "PIN Code is required.")]
        //[RegularExpression(@"^\d{4,6}$", ErrorMessage = "PIN Code must be a numeric value consisting of 4 to 6 digits.")]
        //public string PinCode { get; set; }


        [Required(ErrorMessage = "Date of birth is required.")]
        [AgeValidation(ErrorMessage = "You must be at least 18 years old.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public enAccountType accountType { get; set; }
       
        [ValidationImage(".png,.jpg ,.jpeg")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageUrl { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should be between 5 and 20 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public enUserRole UserRole { get; set; }

        public enGendder Gender { get; set; }

        // Conditionally show Salary field for employees only
        [SalaryValidation(ErrorMessage = "Salary is required for employees.")]
        public int? Salary { get; set; }

        [BranchValidation(ErrorMessage = "Branch is required for employees.")]
        public enBranches? BranchName { get; set; }
    }
}
