using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Ascents;

namespace BoulderBox.Services.Data.Boulders
{
    public class AscentsService : BaseService<Ascent>, IAscentsService
    {
        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;

        public AscentsService(IDeletableEntityRepository<Ascent> ascentsRepository)
            : base(ascentsRepository)
        {
            this.ascentsRepository = ascentsRepository;
        }

        public async Task Create(AscentInputModel ascentInput)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var ascent = mapper.Map<Ascent>(ascentInput);

            await this.ascentsRepository.AddAsync(ascent);
            await this.ascentsRepository.SaveChangesAsync();
        }
    }
}
