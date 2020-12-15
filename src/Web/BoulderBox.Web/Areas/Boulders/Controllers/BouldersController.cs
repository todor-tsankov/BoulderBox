using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Places.Cities;
using BoulderBox.Web.ViewModels.Places.Countries;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Boulders.Controllers
{
    [Area("Boulders")]
    public class BouldersController : BaseController
    {
        private readonly IBouldersService bouldersService;
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;
        private readonly IGymsService gymsService;
        private readonly IGradesService gradesService;
        private readonly ICloudinaryService cloudinaryService;

        public BouldersController(
            IBouldersService bouldersService,
            ICountriesService countriesService,
            ICitiesService citiesService,
            IGymsService gymsService,
            IGradesService gradesService,
            ICloudinaryService cloudinaryService)
        {
            this.bouldersService = bouldersService;
            this.countriesService = countriesService;
            this.citiesService = citiesService;
            this.gymsService = gymsService;
            this.gradesService = gradesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Index(SortingInputModel sorting, int pageId = 1)
        {
            if (sorting.OrderBy == null)
            {
                sorting = new SortingInputModel()
                {
                    OrderBy = "Date",
                    Ascending = false,
                };
            }

            var orderBySelector = GetOrderBySelector(sorting);
            var itemsPerPage = 8;
            var skip = itemsPerPage * (pageId - 1);

            var bouldersViewModel = new BouldersViewModel()
            {
                Boulders = this.bouldersService
                    .GetMany<BoulderViewModel>(
                        orderBySelector: orderBySelector,
                        asc: sorting.Ascending,
                        skip: skip,
                        take: itemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.bouldersService.Count(), itemsPerPage),
                    Sorting = sorting,
                },
            };

            return this.View(bouldersViewModel);
        }

        public IActionResult Details(string id)
        {
            var boulder = this.bouldersService
                .GetSingle<BoulderDetailsViewModel>(x => x.Id == id);

            return this.View(boulder);
        }

        [Authorize]
        public IActionResult Create()
        {
            var boulder = new BoulderInputModel();
            this.SetCreateListItems(boulder);

            return this.View(boulder);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BoulderInputModel boulderInput)
        {
            if (!this.ModelState.IsValid)
            {
                this.SetCreateListItems(boulderInput);
                return this.View(boulderInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var image = await this.cloudinaryService.SaveImageAsync(boulderInput.FormFile);
            await this.bouldersService.AddAsync(boulderInput, userId, image);

            return this.RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var valid = this.bouldersService
                .Exists(x => x.Id == id && x.AuthorId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            var boulder = new BoulderEditViewModel()
            {
                Id = id,
                BoulderInput = this.bouldersService
                    .GetSingle<BoulderInputModel>(x => x.Id == id),
            };

            this.SetEditListItems(boulder.BoulderInput);

            return this.View(boulder);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, BoulderInputModel boulderInput)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var valid = this.bouldersService
                .Exists(x => x.Id == id && x.AuthorId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            if (!this.ModelState.IsValid)
            {
                var boulder = new BoulderEditViewModel()
                {
                    Id = id,
                    BoulderInput = boulderInput,
                };

                this.SetEditListItems(boulder.BoulderInput);

                return this.View(boulder);
            }

            var image = await this.cloudinaryService.SaveImageAsync(boulderInput.FormFile);
            await this.bouldersService.EditAsync(id, boulderInput, image);

            return this.RedirectToAction("Index", "Boulders", new { area = "Boulders" });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var valid = this.bouldersService
                .Exists(x => x.Id == id && x.AuthorId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            await this.bouldersService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "Boulders", new { area = "Boulders" });
        }

        private static Expression<Func<Boulder, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<Boulder, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Date" => x => x.CreatedOn,
                "Grade" => x => x.Grade.Text,
                "Name" => x => x.Name,
                "AscentsCount" => x => x.Ascents.Count,
                _ => x => x.CreatedOn,
            };

            return orderBySelect;
        }

        private void SetEditListItems(BoulderInputModel boulder)
        {
            boulder.CountriesSelectItems = this.countriesService
                            .GetMany<CountryViewModel>(x => x.Cities.Any(y => y.Gyms.Any()), x => x.Name)
                            .Select(x => new SelectListItem()
                            {
                                Value = x.Id,
                                Text = x.Name,
                            })
                            .ToList();

            boulder.CitiesSelectItems = this.citiesService
                .GetMany<CityViewModel>(x => x.CountryId == boulder.CountryId && x.Gyms.Any(), x => x.Name)
                .Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.Name,
                })
                .ToList();

            boulder.GymsSelectItems = this.gymsService
               .GetMany<GymViewModel>(x => x.CityId == boulder.CityId, x => x.Name)
               .Select(x => new SelectListItem()
               {
                   Value = x.Id,
                   Text = x.Name,
               })
               .ToList();

            boulder.GradesSelectItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.Text,
                })
                .ToList();
        }

        private void SetCreateListItems(BoulderInputModel boulder)
        {
            boulder.CountriesSelectItems = this.countriesService
                            .GetMany<CountryViewModel>(x => x.Cities.Any(y => y.Gyms.Any()), x => x.Name)
                            .Select(x => new SelectListItem(x.Name, x.Id))
                            .ToList();

            boulder.GradesSelectItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem(x.Text, x.Id))
                .ToList();
        }
    }
}
