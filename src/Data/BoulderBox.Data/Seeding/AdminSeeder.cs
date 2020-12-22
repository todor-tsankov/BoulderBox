using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BoulderBox.Common;
using BoulderBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BoulderBox.Data.Seeding
{
    public class AdminSeeder : ISeeder
    {
        private readonly IConfiguration configuration;

        public AdminSeeder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var admin = new ApplicationUser()
            {
                Use
            };

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await userManager.CreateAsync(admin, "");

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var administratorRole = await roleManager.FindByNameAsync(GlobalConstants.AdministratorRoleName);


            applicationUser.Roles.Add(administratorRole);

            userManager.CreateAsync(applicationUser);
        }
    }
}
