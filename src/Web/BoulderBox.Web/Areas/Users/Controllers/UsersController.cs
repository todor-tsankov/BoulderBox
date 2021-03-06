﻿using BoulderBox.Services.Data.Users;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Users.Users;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class UsersController : BaseController
    {
        private readonly IApplicationUsersService usersService;

        public UsersController(IApplicationUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Details(string id)
        {
            var existsUser = this.usersService
                .Exists(x => x.Id == id);

            if (!existsUser)
            {
                return this.NotFound();
            }

            var user = this.usersService
                .GetSingle<UserDetailsViewModel>(x => x.Id == id);

            user.Ascents = this.usersService
                .GetGrouped(id);

            return this.View(user);
        }
    }
}
