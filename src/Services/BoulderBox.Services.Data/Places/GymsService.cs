using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

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
    }
}
