using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Gyms
{
    public class GymDetailsBoulderViewModel : IMapFrom<Boulder>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string GradeText { get; set; }

        public DateTime CreatedOn { get; set; }

        public int AscentsCount { get; set; }
    }
}
