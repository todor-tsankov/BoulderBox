using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Boulders
{
    public class GradesService : BaseService<Grade>, IGradesService
    {
        public GradesService(IDeletableEntityRepository<Grade> gradesRepository, IMapper mapper)
            : base(gradesRepository, mapper)
        {
        }
    }
}
