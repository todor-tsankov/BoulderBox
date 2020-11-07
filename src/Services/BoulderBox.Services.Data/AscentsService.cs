using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class AscentsService : BaseService<Ascent>, IAscentsService
    {
        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;

        public AscentsService(IDeletableEntityRepository<Ascent> ascentsRepository)
            : base(ascentsRepository)
        {
            this.ascentsRepository = ascentsRepository;
        }
    }
}
