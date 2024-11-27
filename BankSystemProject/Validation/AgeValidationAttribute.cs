using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Validation
{
   
        public class AgeValidationAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime dateOfBirth)
                {
                //var today = DateOnly.FromDateTime(DateTime.Now);
                var today = DateTime.Today;    
                var age = today.Year - dateOfBirth.Year;

                    // If today's date is before the birthday, subtract one from age
                    if (today < dateOfBirth.AddYears(age))
                        age--;

                    return age >= 18; // Only valid if the user is 18 or older
                }

                return false; // Invalid if the value is not a valid DateOnly
            }
        }
}
