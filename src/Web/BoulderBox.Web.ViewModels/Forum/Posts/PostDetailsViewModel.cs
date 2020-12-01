using System;
using System.Collections.Generic;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum.Posts
{
    public class PostDetailsViewModel : IMapFrom<Post>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string ImageSource { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
