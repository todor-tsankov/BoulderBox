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

            var grades = new List<Grade>()
            {
                new Grade() { Text = "9A", Points = 1300 },
                new Grade() { Text = "8C+", Points = 1250 },
                new Grade() { Text = "8C", Points = 1200 },
                new Grade() { Text = "8B+", Points = 1150 },
                new Grade() { Text = "8B", Points = 1100 },
                new Grade() { Text = "8A+", Points = 1050 },
                new Grade() { Text = "8A", Points = 1000 },
                new Grade() { Text = "7C+", Points = 950 },
                new Grade() { Text = "7C", Points = 900 },
                new Grade() { Text = "7B+", Points = 850 },
                new Grade() { Text = "7B", Points = 800 },
                new Grade() { Text = "7A+", Points = 750 },
                new Grade() { Text = "7A", Points = 700 },
                new Grade() { Text = "6C+", Points = 650 },
                new Grade() { Text = "6C", Points = 600 },
                new Grade() { Text = "6B+", Points = 550 },
                new Grade() { Text = "6B", Points = 500 },
                new Grade() { Text = "6A+", Points = 450 },
                new Grade() { Text = "6A", Points = 400 },
                new Grade() { Text = "5C", Points = 350 },
                new Grade() { Text = "5B", Points = 300 },
                new Grade() { Text = "5A", Points = 250 },
                new Grade() { Text = "4", Points = 200 },
                new Grade() { Text = "3", Points = 100 },
            };

            await dbContext.Grades.AddRangeAsync(grades);
            await dbContext.SaveChangesAsync();
        }
    }
}
