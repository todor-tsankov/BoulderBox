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

            var results = new List<SearchViewModel>();

            var countries = this.countriesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));
            
            results.AddRange(countries);
            this.SetProperties(countries, "Places", "Countries", "Details");

            var cities = this.citiesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            results.AddRange(cities);
            this.SetProperties(cities, "Places", "Cities", "Details");

            var gyms = this.gymsService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            results.AddRange(gyms);
            this.SetProperties(gyms, "Places", "Gyms", "Details");

            var boulders = this.bouldersService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            results.AddRange(boulders);
            this.SetProperties(boulders, "Boulders", "Boulders", "Details");

            var categories = this.categoriesService
                .GetMany<SearchViewModel>(x => x.Name.Contains(text));

            results.AddRange(categories);
            this.SetProperties(categories, "Forum", "Categories", "Details");

            var posts = this.postsService
                .GetMany<SearchViewModel>(x => x.Title.Contains(text));

            results.AddRange(posts);
            this.SetProperties(posts, "Forum", "Posts", "Details");

            return results;
        }

        private void SetProperties(IEnumerable<SearchViewModel> searchViewModels, string area, string controller, string action)
        {
            foreach (var viewModel in searchViewModels)
            {
                viewModel.Area = area;
                viewModel.Controller = controller;
                viewModel.Action = action;
            }
        }
    }
}
