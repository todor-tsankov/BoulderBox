using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Boulders
{
    public class StylesService : BaseService<Style>, IStylesService
    {
        private readonly IDeletableEntityRepository<Style> stylesRepository;

        public StylesService(IDeletableEntityRepository<Style> stylesRepository)
            : base(stylesRepository)
        {
            this.stylesRepository = stylesRepository;
        }
    }
}
