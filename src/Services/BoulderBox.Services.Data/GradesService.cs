using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class GradesService : BaseService<Grade>, IGradesService
    {
        public GradesService(IDeletableEntityRepository<Grade> gradesRepository)
            : base(gradesRepository)
        {
        }
    }
}
