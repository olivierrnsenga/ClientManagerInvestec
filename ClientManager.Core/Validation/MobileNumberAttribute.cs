using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ClientManager.Core.Validation
{
    public class MobileNumberAttribute : ValidationAttribute
    {
        private const string MobileNumberPattern = @"^(\+\d{11}|\d{10})$";

        public MobileNumberAttribute()
        {
            ErrorMessage = "Invalid Mobile Number format.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mobileNumber = value as string;

            if (string.IsNullOrEmpty(mobileNumber))
            {
                return ValidationResult.Success;
            }

            if (!IsValidMobileNumber(mobileNumber))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool IsValidMobileNumber(string mobileNumber)
        {
            return Regex.IsMatch(mobileNumber, MobileNumberPattern);
        }
    }
}