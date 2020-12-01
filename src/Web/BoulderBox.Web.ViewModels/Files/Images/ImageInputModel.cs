using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Files.Images
{
    public class ImageInputModel : IMapTo<Image>
    {
        public string Id { get; set; }

        public string Source { get; set; }
    }
}
