using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoulderBox.Web.ViewModels.Common
{
    public class SortingInputModel
    {
        public string OrderBy { get; set; }

        public string Where { get; set; }

        public bool Ascending { get; set; }

        public IDictionary<string, string> GetAllRouteData(int pageId)
        {
            return new Dictionary<string, string>()
            {
                { "pageId", pageId.ToString() },
                { "orderBy", this.OrderBy },
                { "where", this.Where },
                { "ascending", this.Ascending.ToString() },
            };
        }
    }
}
