using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_Login
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
  
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
