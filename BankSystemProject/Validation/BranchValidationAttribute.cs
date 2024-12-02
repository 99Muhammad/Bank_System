using BankSystemProject.Models.DTOs;
using BankSystemProject.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankSystemProject.Validation
{
    public class BranchValidationAttribute: ValidationAttribute
    {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
               
                var objectInstance = validationContext.ObjectInstance as Req_Registration;

                if (objectInstance == null)
                {
                    return new ValidationResult("Invalid object type.");
                }

                if (enUserRole.Customer != objectInstance.UserRole
                    && (value == null || !((enBranches?)value).HasValue))
                {
                    return new ValidationResult($"Branch name is required for {objectInstance.UserRole.ToString()}.");
                }


                return ValidationResult.Success;
            }
        }
}
