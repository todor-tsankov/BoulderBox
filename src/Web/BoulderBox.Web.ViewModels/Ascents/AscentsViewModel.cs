using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Ascents
{
    public class AscentsViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<AscentViewModel> Ascents { get; set; }
    }
}
