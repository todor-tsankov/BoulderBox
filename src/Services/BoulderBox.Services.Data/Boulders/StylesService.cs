using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Boulders
{
    public class StylesService : BaseService<Style>, IStylesService
    {
        public StylesService(IDeletableEntityRepository<Style> stylesRepository, IMapper mapper)
            : base(stylesRepository, mapper)
        {
        }
    }
}
