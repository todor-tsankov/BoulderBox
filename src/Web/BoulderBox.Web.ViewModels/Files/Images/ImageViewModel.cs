using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Files.Images
{
    public class ImageViewModel : IMapFrom<Image>
    {
        public string Source { get; set; }
    }
}
