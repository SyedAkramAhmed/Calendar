using System;
using System.ComponentModel.DataAnnotations;

namespace Calendar.API.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EmailArrayRequiredValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string[] array)
            {
                if (array.Length == 0) return new ValidationResult("There should be atleast 1 required attendee.");
                EmailAddressAttribute emailAttribute = new EmailAddressAttribute();
                foreach (string str in array)
                {
                    if (!emailAttribute.IsValid(str))
                    {
                        return new ValidationResult("One or More attendees is not a valid email address.");
                    }
                }
                return ValidationResult.Success;
            }
            return base.IsValid(value, validationContext);
        }
    }
    public class EmailArrayNotRequiredValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string[] array)
            {
                EmailAddressAttribute emailAttribute = new EmailAddressAttribute();
                foreach (string str in array)
                {
                    if (!emailAttribute.IsValid(str))
                    {
                        return new ValidationResult("One or More attendees is not a valid email address.");
                    }
                }
                return ValidationResult.Success;
            }
            return base.IsValid(value, validationContext);
        }
    }
}
