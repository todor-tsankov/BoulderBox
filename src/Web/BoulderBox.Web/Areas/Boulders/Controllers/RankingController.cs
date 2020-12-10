using System;
using System.Linq.Expressions;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Users;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Boulders.Ranking;
using BoulderBox.Web.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Boulders.Controllers
{
    [Area("Boulders")]
    public class RankingController : BaseController
    {
        private readonly IApplicationUsersService applicationUsersService;

        public RankingController(IApplicationUsersService applicationUsersService)
        {
            this.applicationUsersService = applicationUsersService;
        }

        public IActionResult Index(SortingInputModel sorting, int pageId = 1)
        {
            if (sorting.OrderBy == null)
            {
                sorting = new SortingInputModel()
                {
                    OrderBy = "Date",
                };
            }

            var orderBySelector = GetOrderBySelector(sorting);
            var skip = DefaultItemsPerPage * (pageId - 1);

            var rankingsViewModel = new RankingsViewModel()
            {
                StartRank = skip + 1,

                Ranking = this.applicationUsersService
                    .GetMany<RankingViewModel>(
                        orderBySelector: orderBySelector,
                        asc: false,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.applicationUsersService.Count()),
                    Sorting = sorting,
                },
            };

            return this.View(rankingsViewModel);
        }

        private static Expression<Func<ApplicationUser, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<ApplicationUser, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Weekly" => x => x.Points.Weekly,
                "Monthly" => x => x.Points.Monthly,
                "Yearly" => x => x.Points.Yearly,
                "AllTime" => x => x.Points.AllTime,

                _ => x => x.Points.Weekly,
            };

            return orderBySelect;
        }
    }
}
