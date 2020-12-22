using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace BoulderBox.Web.ViewModels.ValidationAttributes
{
    public class ImageAttribute : ValidationAttribute
    {
        public const string Png = "PNG";
        public const string Jpg = "JPG";
        public const string Jpeg = "JPEG";

        public const string AcceptedFormatsMessage = "Only .jpeg, .jpg and .png formats are accepted.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not IFormFile image)
            {
                return new ValidationResult(AcceptedFormatsMessage);
            }

            var format = Image.DetectFormat(image.OpenReadStream());

            if (format == null ||
                (format.Name != Png &&
                 format.Name != Jpg &&
                 format.Name != Jpeg))
            {
                return new ValidationResult(AcceptedFormatsMessage);
            }

            return ValidationResult.Success;
        }
    }
}
