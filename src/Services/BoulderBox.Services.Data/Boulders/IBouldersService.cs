﻿using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Boulders
{
    public interface IBouldersService : IBaseService<Boulder>
    {
        Task<bool> AddBoulderAsync(BoulderInputModel boulderInput, ImageInputModel image);
    }
}
