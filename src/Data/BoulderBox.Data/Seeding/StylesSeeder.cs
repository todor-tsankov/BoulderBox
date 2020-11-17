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

            var stylesJson = await File.ReadAllTextAsync(@"../../Data/BoulderBox.Data/Seeding/Data/Styles.json");
            var styles = JsonConvert.DeserializeObject<IEnumerable<Style>>(stylesJson);

            await dbContext.Styles.AddRangeAsync(styles);
            await dbContext.SaveChangesAsync();
        }
    }
}
