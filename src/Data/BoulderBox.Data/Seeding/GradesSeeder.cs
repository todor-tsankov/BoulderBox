using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Data.Models;
using Newtonsoft.Json;

namespace BoulderBox.Data.Seeding
{
    public class GradesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Grades.Any())
            {
                return;
            }

            var gradesJson = File.ReadAllText(@"../../Data/BoulderBox.Data/Seeding/Data/Grades.json");
            var grades = JsonConvert.DeserializeObject<IEnumerable<Grade>>(gradesJson);

            await dbContext.Grades.AddRangeAsync(grades);
            await dbContext.SaveChangesAsync();
        }
    }
}
