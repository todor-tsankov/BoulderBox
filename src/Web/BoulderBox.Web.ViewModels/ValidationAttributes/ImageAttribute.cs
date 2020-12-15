using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace BoulderBox.Web.ViewModels.ValidationAttributes
{
    public class ImageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile image)
            {
                return ValidationResult.Success;
            }

            var format = Image.DetectFormat(image.OpenReadStream());

            if (format == null ||
                (format.Name != "JPEG" &&
                 format.Name != "JPG" &&
                 format.Name != "PNG"))
            {
                return new ValidationResult("Only .jpeg, .jpg and .png formats are accepted.");
            }

            return ValidationResult.Success;
        }
    }
}
