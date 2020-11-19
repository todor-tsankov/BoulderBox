using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Styles
{
    public class StyleViewModel : IMapFrom<Style>
    {
        public string Id { get; set; }

        public string ShortText { get; set; }

        public string LongText { get; set; }

        public int BonusPoints { get; set; }

        public string Description { get; set; }
    }
}
