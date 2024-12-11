using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class ForgotPasswordReqDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
