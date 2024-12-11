using BankSystemProject.Shared.Enums;
using BankSystemProject.Validation;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_UpdateEmployeeInfoDto
    {
        
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


        [ValidationImage(".png,.jpg ,.jpeg")]
        [DataType(DataType.Upload)]
        // public IFormFile? ImageUrl { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username should be between 5 and 20 characters.")]
        public string UserName { get; set; }
  
      
    }
}
