using System.Collections.Generic;

namespace BoulderBox.Web.ViewModels.Search
{
    public class SearchResultsViewModel
    {
        public string Text { get; set; }

        public IEnumerable<SearchViewModel> Results { get; set; }
    }
}
