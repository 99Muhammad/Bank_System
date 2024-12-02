using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface.AdminInterfaces;
using BankSystemProject.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Validation
{
    public class SalaryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
          
            var objectInstance = validationContext.ObjectInstance as Req_Registration;

            if (objectInstance == null)
            {
                return new ValidationResult("Invalid object type.");
            }

            if ( enUserRole.Customer!=objectInstance.UserRole
                && (value == null || !((int?)value).HasValue))
            {
                return new ValidationResult($"Salary is required for {objectInstance.UserRole.ToString()}.");
            }


            return ValidationResult.Success;
        }
    }




}
