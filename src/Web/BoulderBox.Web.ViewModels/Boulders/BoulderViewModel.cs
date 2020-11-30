using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders
{
    public class BoulderViewModel : IMapFrom<Boulder>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string GymId { get; set; }

        public string GymName { get; set; }

        public string GradeText { get; set; }

        public string ImageSource { get; set; }
    }
}
