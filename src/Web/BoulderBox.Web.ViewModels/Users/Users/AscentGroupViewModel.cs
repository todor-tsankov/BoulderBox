using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Users.Users
{
    public class AscentGroupViewModel
    {
        public string Key { get; set; }

        public IEnumerable<UserDetailsAscentViewModel> Ascents { get; set; }
    }
}
