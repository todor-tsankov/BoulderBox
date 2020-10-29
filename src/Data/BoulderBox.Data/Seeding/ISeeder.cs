﻿using System;
using System.Threading.Tasks;

namespace BoulderBox.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
