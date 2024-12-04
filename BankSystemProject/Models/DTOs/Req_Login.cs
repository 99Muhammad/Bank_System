using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_Login
    {
        //[Required(ErrorMessage = "UserName is required.")]
        //[EmailAddress(ErrorMessage = "Invalid UserName format.")]
        //public string UserName { get; set; }

        [Required]
        public string UserName { get; set; }
  
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
