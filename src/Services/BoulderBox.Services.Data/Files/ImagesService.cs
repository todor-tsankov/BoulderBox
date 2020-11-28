using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Files
{
    public class ImagesService : BaseService<Image>, IImagesService
    {
        private readonly IDeletableEntityRepository<Image> imagesRepository;
        private readonly IMapper mapper;

        public ImagesService(IDeletableEntityRepository<Image> imagesRepository, IMapper mapper)
            : base(imagesRepository, mapper)
        {
            this.imagesRepository = imagesRepository;
            this.mapper = mapper;
        }
    }
}
