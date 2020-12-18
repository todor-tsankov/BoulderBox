using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Boulders.Styles;

namespace BoulderBox.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<GradeViewModel> Grades { get; set; }

        public IEnumerable<StyleViewModel> Styles { get; set; }
    }
}
