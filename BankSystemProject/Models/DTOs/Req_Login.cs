using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_Login
    {
        [Required]
        public string UserName { get; set; }
  
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
