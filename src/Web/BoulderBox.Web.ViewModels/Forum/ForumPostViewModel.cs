using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum
{
    public class ForumPostViewModel : IMapFrom<ForumPost>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string ImageSource { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
