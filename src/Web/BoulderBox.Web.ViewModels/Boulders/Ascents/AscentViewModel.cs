using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders.Ascents
{
    public class AscentViewModel : IMapFrom<Ascent>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUsername { get; set; }

        public string BoulderId { get; set; }

        public string BoulderName { get; set; }

        public string GradeText { get; set; }

        public string StyleShortText { get; set; }

        public int? Stars { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }
    }
}
