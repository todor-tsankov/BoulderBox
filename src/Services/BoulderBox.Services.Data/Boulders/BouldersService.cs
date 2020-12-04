using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Files.Images;

namespace BoulderBox.Services.Data.Boulders
{
    public class BouldersService : BaseService<Boulder>, IBouldersService
    {
        private readonly IDeletableEntityRepository<Boulder> bouldersRepository;
        private readonly IMapper mapper;

        public BouldersService(IDeletableEntityRepository<Boulder> bouldersRepository, IMapper mapper)
            : base(bouldersRepository, mapper)
        {
            this.bouldersRepository = bouldersRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(BoulderInputModel boulderInput, string authorId, ImageInputModel imageInput)
        {
            this.NullCheck(boulderInput, nameof(boulderInput));
            this.NullCheck(authorId, nameof(authorId));
            this.NullCheck(imageInput, nameof(imageInput));

            var image = this.mapper.Map<Image>(imageInput);
            var boulder = this.mapper.Map<Boulder>(boulderInput);

            boulder.AuthorId = authorId;
            boulder.Image = image;

            await this.bouldersRepository.AddAsync(boulder);
            await this.bouldersRepository.SaveChangesAsync();

            return true;
        }
    }
}
