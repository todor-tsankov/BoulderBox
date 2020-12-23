using System.Linq;
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
            this.NullCheck(mapper, nameof(mapper));
            this.NullCheck(bouldersRepository, nameof(bouldersRepository));

            this.mapper = mapper;
            this.bouldersRepository = bouldersRepository;
        }

        public async Task AddAsync(BoulderInputModel boulderInput, string authorId, ImageInputModel imageInput)
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
        }

        public async Task EditAsync(string id, BoulderInputModel boulderInput, ImageInputModel imageInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(boulderInput, nameof(boulderInput));

            var boulder = this.bouldersRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (imageInput != null)
            {
                boulder.Image = this.mapper.Map<Image>(imageInput);
            }

            boulder.Name = boulderInput.Name;
            boulder.Description = boulderInput.Description;
            boulder.GradeId = boulderInput.GradeId;
            boulder.GymId = boulderInput.GymId;

            await this.bouldersRepository.SaveChangesAsync();
        }
    }
}
