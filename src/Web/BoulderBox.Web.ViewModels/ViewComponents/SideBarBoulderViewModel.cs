using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ViewComponents
{
    public class SideBarBoulderViewModel : IMapFrom<Boulder>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string GradeText { get; set; }
    }
}
