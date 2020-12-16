using System.Collections.Generic;
using System.Linq;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;
        private readonly IGymsService gymsService;
        private readonly IBouldersService bouldersService;
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public SearchController(
            ICountriesService countriesService,
            ICitiesService citiesService,
            IGymsService gymsService,
            IBouldersService bouldersService,
            ICategoriesService categoriesService,
            IPostsService postsService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
            this.gymsService = gymsService;
            this.bouldersService = bouldersService;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult Index(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            var searchResults = new SearchResultsViewModel()
            {
                Text = text,
                Results = this.GetResults(text),
            };

            return this.View(searchResults);
        }

        private IList<SearchViewModel> GetResults(string text)
        {
            if (text == null || text == string.Empty)
            {
                return new List<SearchViewModel>();
            }

            var countries = this.countriesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            foreach (var country in countries)
            {
                country.Area = "Places";
                country.Controller = "Countries";
                country.Action = "Details";
            }

            var cities = this.citiesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            foreach (var city in cities)
            {
                city.Area = "Places";
                city.Controller = "Cities";
                city.Action = "Details";
            }

            var gyms = this.gymsService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            foreach (var gym in gyms)
            {
                gym.Area = "Places";
                gym.Controller = "Gyms";
                gym.Action = "Details";
            }

            var boulders = this.bouldersService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            foreach (var boulder in boulders)
            {
                boulder.Area = "Boulders";
                boulder.Controller = "Boulders";
                boulder.Action = "Details";
            }

            var categories = this.categoriesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            foreach (var category in categories)
            {
                category.Area = "Forum";
                category.Controller = "Categories";
                category.Action = "Details";
            }

            var posts = this.postsService
                .GetMany<SearchViewModel>(x => x.Title.Contains(text));

            foreach (var post in posts)
            {
                post.Area = "Forum";
                post.Controller = "Posts";
                post.Action = "Details";
            }

            var results = new List<SearchViewModel>();

            results.AddRange(countries);
            results.AddRange(cities);
            results.AddRange(gyms);
            results.AddRange(boulders);
            results.AddRange(categories);
            results.AddRange(posts);

            return results;
        }
    }
}
