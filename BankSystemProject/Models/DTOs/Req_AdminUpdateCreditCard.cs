using BankSystemProject.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_AdminUpdateCreditCard
    {
        [Required]
        public enCreditCardType CardType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Credit Limit must be a positive value.")]
        public double CreditLimit { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        //public double Balance { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public enCreditCardStatus Status { get; set; }

        [Required(ErrorMessage = "PIN Code is required.")]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "PIN Code must be a numeric value consisting of 4 to 6 digits.")]
        public string PinCode { get; set; }
    }
}
