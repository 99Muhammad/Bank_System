using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Validation
{
    public class SalaryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var registration = (Req_UpdateUserInfo)validationContext.ObjectInstance;

            ////var registration = (Req_Registration)validationContext.ObjectInstance;

            //if (value == null || !((double?)value).HasValue)
            //{
            //    return new ValidationResult("Salary is required for employees.");
            //}

            //return ValidationResult.Success;


            var objectInstance = validationContext.ObjectInstance as IUser;

            if (objectInstance == null)
            {
                return new ValidationResult("Invalid object type.");
            }

            if (value == null || !((double?)value).HasValue)
            {
                return new ValidationResult("Salary is required.");
            }

            return ValidationResult.Success;
        }
    }

}
