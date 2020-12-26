using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Data.Models;
using Newtonsoft.Json;

namespace BoulderBox.Data.Seeding
{
    public class StylesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Styles.Any())
            {
                return;
            }

            var styles = new List<Style>()
            {
                new Style() { ShortText = "RP", LongText = "Redpoint", BonusPoints = 0 },
                new Style() { ShortText = "FL", LongText = "Flash", BonusPoints = 53 },
                new Style() { ShortText = "OS", LongText = "On-sight", BonusPoints = 145 },
            };

            await dbContext.Styles.AddRangeAsync(styles);
            await dbContext.SaveChangesAsync();
        }
    }
}
