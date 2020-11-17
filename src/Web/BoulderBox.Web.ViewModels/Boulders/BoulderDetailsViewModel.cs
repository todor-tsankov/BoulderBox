using System;
using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders
{
    public class BoulderDetailsViewModel : IMapFrom<Boulder>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AuthorId { get; set; }

        public string AuthorEmail { get; set; }

        public string GymId { get; set; }

        public string GymName { get; set; }

        public string GradeText{ get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<BoulderDetailsAscentViewModel> Ascents { get; set; }
    }
}
