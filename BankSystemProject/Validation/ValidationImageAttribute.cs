using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BankSystemProject.Validation
{
    public class ValidationImageAttribute : ValidationAttribute
    {
        private readonly string _allowedExtensions;

        public ValidationImageAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                // Get the file extension
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

                // Check if the extension is allowed
                var allowedExtensions = _allowedExtensions.Split(',');

                foreach (var allowedExtension in allowedExtensions)
                {
                    if (extension == allowedExtension.Trim().ToLowerInvariant())
                    {
                        return true;
                    }
                }

                // If the extension is not allowed, return false
                ErrorMessage = $"The file extension is not valid. Allowed extensions are: {_allowedExtensions}";
                return false;
            }

            return true; // Valid if no file is provided (optional)
        }
    }
}
