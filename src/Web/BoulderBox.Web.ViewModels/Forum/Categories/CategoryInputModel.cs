using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum.Categories
{
    public class CategoryInputModel : IMapTo<Category>
    {
        [Display(Name = "Name *")]
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(2, ErrorMessage = "Name must be between 2 and 50 characters long.")]
        [MaxLength(50, ErrorMessage = "Name must be between 2 and 50 characters long.")]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
