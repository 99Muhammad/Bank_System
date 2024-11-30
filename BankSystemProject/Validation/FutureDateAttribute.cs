using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Validation
{
    public class FutureDateAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue <= DateTime.UtcNow)
                {
                    return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
