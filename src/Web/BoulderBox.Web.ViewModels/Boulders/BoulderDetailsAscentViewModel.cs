using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders
{
    public class BoulderDetailsAscentViewModel : IMapFrom<Ascent>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string GradeText { get; set; }

        public string StyleShortText { get; set; }

        public string StyleLongText { get; set; }

        public bool? Recommend { get; set; }

        public int? Stars { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }
    }
}
