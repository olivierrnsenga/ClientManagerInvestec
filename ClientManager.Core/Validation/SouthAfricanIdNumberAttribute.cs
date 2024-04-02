using System.ComponentModel.DataAnnotations;


namespace ClientManager.Core.Validation
{
    public class SouthAfricanIdNumberAttribute : ValidationAttribute
    {
        public SouthAfricanIdNumberAttribute()
        {
            ErrorMessage = "Invalid South African ID Number.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var idNumber = value as string;

            if (string.IsNullOrEmpty(idNumber))
            {
                return ValidationResult.Success; 
            }

            var idValidator = new SaIdNumberValidator(idNumber);
            if (!idValidator.IsValid())
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}