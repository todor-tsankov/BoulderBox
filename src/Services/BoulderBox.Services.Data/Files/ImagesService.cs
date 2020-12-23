using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Files
{
    public class ImagesService : BaseService<Image>, IImagesService
    {
        public ImagesService(IDeletableEntityRepository<Image> imagesRepository, IMapper mapper)
            : base(imagesRepository, mapper)
        {
        }
    }
}
