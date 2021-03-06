﻿using System;
using System.Linq.Expressions;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Area("Places")]
    public class GymsController : BaseController
    {
        private readonly IGymsService gymsService;
        private readonly IBouldersService bouldersService;

        public GymsController(IGymsService gymsService, IBouldersService bouldersService)
        {
            this.gymsService = gymsService;
            this.bouldersService = bouldersService;
        }

        public IActionResult Index(SortingInputModel sorting, int pageId = 1)
        {
            if (sorting.OrderBy == null)
            {
                sorting = new SortingInputModel()
                {
                    Ascending = true,
                    OrderBy = "Name",
                };
            }

            var orderBySelector = GetOrderBySelector(sorting);
            var skip = DefaultItemsPerPage * (pageId - 1);

            var gymsViewModel = new GymsViewModel()
            {
                Gyms = this.gymsService
                    .GetMany<GymViewModel>(
                        orderBySelector: orderBySelector,
                        asc: sorting.Ascending,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.gymsService.Count()),
                    Sorting = sorting,
                },
            };

            return this.View(gymsViewModel);
        }

        public IActionResult Details(string id)
        {
            var existsGym = this.gymsService
                .Exists(x => x.Id == id);

            if (!existsGym)
            {
                return this.NotFound();
            }

            var gym = this.gymsService
                .GetSingle<GymDetailsViewModel>(x => x.Id == id);

            gym.Boulders = this.bouldersService
                .GetMany<GymDetailsBoulderViewModel>(x => x.GymId == id, x => x.CreatedOn, false);

            return this.View(gym);
        }

        private static Expression<Func<Gym, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<Gym, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Name" => x => x.Name,
                "BouldersCount" => x => x.Boulders.Count,
                _ => x => x.Name,
            };

            return orderBySelect;
        }
    }
}
