using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class ImagesService : BaseService<Image>, IImagesService
    {
        public ImagesService(IDeletableEntityRepository<Image> imagesRepository)
            : base(imagesRepository)
        {
        }
    }
}
