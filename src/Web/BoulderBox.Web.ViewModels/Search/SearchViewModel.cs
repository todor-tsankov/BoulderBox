using System;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Search
{
    public class SearchViewModel : IHaveCustomMappings,
        IMapFrom<Country>,
        IMapFrom<City>,
        IMapFrom<Gym>,
        IMapFrom<Boulder>,
        IMapFrom<Category>,
        IMapFrom<Post>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Category => this.Controller.Replace("Controller", string.Empty);

        public string GetBeoreText(string text)
        {
            var index = this.Name
                .IndexOf(text, StringComparison.InvariantCultureIgnoreCase);

            return this.Name
                .Remove(index);
        }

        public string GetAfterText(string text)
        {
            var index = this.Name
                .IndexOf(text, StringComparison.InvariantCultureIgnoreCase);

            var result = this.Name[(index + text.Length)..];

            return result;
        }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, SearchViewModel>()
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Title));
        }
    }
}
