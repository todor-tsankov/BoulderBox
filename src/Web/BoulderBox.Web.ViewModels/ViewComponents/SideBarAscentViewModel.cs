using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ViewComponents
{
    public class SideBarAscentViewModel : IMapFrom<Ascent>
    {
        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string GradeText { get; set; }

        public string StyleShortText { get; set; }
    }
}
