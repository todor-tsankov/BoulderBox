using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Gyms;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public class GymsService : BaseService<Gym>, IGymsService
    {
        private readonly IDeletableEntityRepository<Gym> gymsRepository;

        public GymsService(IDeletableEntityRepository<Gym> gymsRepository)
            : base(gymsRepository)
        {
            this.gymsRepository = gymsRepository;
        }

        public async Task<bool> AddGymAsync(GymInputModel gymInput, ImageInputModel imageInput)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var country = mapper.Map<Gym>(gymInput);

            country.Image = mapper.Map<Image>(imageInput);

            await this.gymsRepository.AddAsync(country);
            await this.gymsRepository.SaveChangesAsync();

            return true;
        }
    }
}
