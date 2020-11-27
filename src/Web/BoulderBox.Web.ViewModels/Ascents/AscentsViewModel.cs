using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Ascents
{
    public class AscentsViewModel : PaginationBaseViewModel
    {
        public IEnumerable<AscentViewModel> Ascents { get; set; }
    }
}
