using System.Collections.Generic;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Forum.Categories
{
    public class CategoryDetailsViewModel : IMapFrom<Category>
    {
        public CommonViewModel Common { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        [IgnoreMap]
        public IEnumerable<CategoryDetailsForumPostViewModel> Posts { get; set; }
    }
}
