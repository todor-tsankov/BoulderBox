using System;
using System.ComponentModel.DataAnnotations;

namespace BoulderBox.Web.ViewModels.ValidationAttributes
{
    public class AscentDateAtribute : ValidationAttribute
    {
        public const string DateRequiredErrorMessage = "Date is required.";
        public const string DateInTheFutureErrorMessage = "Date cannot be be in the future.";
        public const string DateTooMuchInThePast = "Are you sure climbing gyms existed back then?.";

        public const int DateMinYear = 1950;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime)
            {
                return new ValidationResult(DateRequiredErrorMessage);
            }

            var date = (DateTime)value;

            if (date > DateTime.UtcNow.Date)
            {
                return new ValidationResult(DateInTheFutureErrorMessage);
            }
            else if (date.Year < DateMinYear)
            {
                return new ValidationResult(DateTooMuchInThePast);
            }

            return ValidationResult.Success;
        }
    }
}
