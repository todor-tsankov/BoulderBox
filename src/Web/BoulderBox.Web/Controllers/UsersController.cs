﻿using BoulderBox.Services.Data.Users;
using BoulderBox.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IApplicationUsersService usersService;

        public UsersController(IApplicationUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Details(string id)
        {
            var user = this.usersService
                .GetSingle<UserDetailsViewModel>(x => x.Id == id);

            return this.View(user);
        }
    }
}