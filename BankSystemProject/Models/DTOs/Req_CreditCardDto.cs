using BankSystemProject.Shared.Enums;
using BankSystemProject.Validation;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Models.DTOs
{
    public class Req_CreditCardDto
    {

        [Required(ErrorMessage = "CustomerAccountId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerAccountId must be a positive integer.")]
        public int CustomerAccountId { get; set; }

        [Required(ErrorMessage = "CardType is required.")]
        [EnumDataType(typeof(enCreditCardType), ErrorMessage = "Invalid CardType. Valid types are: Visa, MasterCard, AmericanExpress, Discover.")]
        public enCreditCardType CardType { get; set; }

        [Required(ErrorMessage = "CreditLimit is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "CreditLimit must be greater than 0.")]
        public double CreditLimit { get; set; }

        //[Required(ErrorMessage = "Balance is required.")]
        //[Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative.")]
        //public double Balance { get; set; }

        [Required(ErrorMessage = "ExpiryDate is required.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "ExpiryDate must be a future date.")]
        public DateTime ExpiryDate { get; set; }

        //[Required(ErrorMessage = "Status is required.")]
        //public enCreditCardStatus Status { get; set; }

        //public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "PIN Code is required.")]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "PIN Code must be a numeric value consisting of 4 to 6 digits.")]
        public string PinCode { get; set; }

        //[Required(ErrorMessage = "PinCode is required.")]
        //[StringLength(4, MinimumLength = 4, ErrorMessage = "PinCode must be exactly 4 digits.")]
        //[RegularExpression("^[0-9]{4}$", ErrorMessage = "PinCode must contain only digits.")]
        //public string PinCode { get; set; }
    }
}
