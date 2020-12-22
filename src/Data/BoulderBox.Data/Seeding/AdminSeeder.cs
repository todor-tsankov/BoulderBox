using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var firstAdminEmail = configuration["FirstAdmin:Email"];
            var firstAdminUserName = configuration["FirstAdmin:UserName"];

            var alreadyExists = dbContext.Users
                .Any(x => x.UserName == firstAdminUserName || x.Email == firstAdminEmail);

            if (!alreadyExists)
            {
                var password = configuration["FirstAdmin:Password"];
                await AddAdmin(userManager, firstAdminEmail, firstAdminUserName, password);
            }

            var secondAdminEmail = configuration["SecondAdmin:Email"];
            var secondAdminUserName = configuration["SecondAdmin:UserName"];

            alreadyExists = dbContext.Users
                .Any(x => x.UserName == secondAdminUserName || x.Email == secondAdminEmail);

            if (!alreadyExists)
            {
                var password = configuration["SecondAdmin:Password"];
                await AddAdmin(userManager, secondAdminEmail, secondAdminUserName, password);
            }
        }

        private static async Task AddAdmin(UserManager<ApplicationUser> userManager, string email, string username, string password)
        {
            var admin = new ApplicationUser()
            {
                UserName = username,
                Email = email,
            };

            var success = await userManager.CreateAsync(admin, password);

            if (success.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
