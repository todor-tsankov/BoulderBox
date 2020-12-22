using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders.Boulders
{
    public class BoulderDetailsViewModel : IMapFrom<Boulder>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string GymId { get; set; }

        public string GymName { get; set; }

        public string GradeText { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RecommendCount { get; set; }

        public double AverageStars { get; set; }

        [IgnoreMap]
        public IEnumerable<BoulderDetailsAscentViewModel> Ascents { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Boulder, BoulderDetailsViewModel>()
                .ForMember(x => x.RecommendCount, x => x.MapFrom(y => y.Ascents.Count(x => x.Recommend == true)))
                .ForMember(x => x.AverageStars, x => x.MapFrom(y => y.Ascents.Average(x => x.Stars)));
        }
    }
}
