using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Gyms;

namespace BoulderBox.Services.Data.Places
{
    public class GymsService : BaseService<Gym>, IGymsService
    {
        private readonly IDeletableEntityRepository<Gym> gymsRepository;
        private readonly IMapper mapper;

        public GymsService(IDeletableEntityRepository<Gym> gymsRepository, IMapper mapper)
            : base(gymsRepository, mapper)
        {
            this.gymsRepository = gymsRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(GymInputModel gymInput, ImageInputModel imageInput)
        {
            this.NullCheck(gymInput, nameof(gymInput));

            var country = this.mapper.Map<Gym>(gymInput);
            country.Image = this.mapper.Map<Image>(imageInput);

            await this.gymsRepository.AddAsync(country);
            await this.gymsRepository.SaveChangesAsync();
        }
    }
}
