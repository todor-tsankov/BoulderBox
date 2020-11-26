using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Boulders;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Boulders
{
    public class BouldersService : BaseService<Boulder>, IBouldersService
    {
        private readonly IDeletableEntityRepository<Boulder> bouldersRepository;

        public BouldersService(IDeletableEntityRepository<Boulder> bouldersRepository)
            : base(bouldersRepository)
        {
            this.bouldersRepository = bouldersRepository;
        }

        public async Task<bool> AddBoulderAsync(BoulderInputModel boulderInput, string authorId, ImageInputModel imageInput)
        {
            var mapper = AutoMapperConfig.MapperInstance;

            var image = mapper.Map<Image>(imageInput);
            var boulder = mapper.Map<Boulder>(boulderInput);

            boulder.AuthorId = authorId;
            boulder.Image = image;

            await this.bouldersRepository.AddAsync(boulder);
            await this.bouldersRepository.SaveChangesAsync();

            return true;
        }
    }
}
