using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Boulders.Grades
{
    public class GradeViewModel : IMapFrom<Grade>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Points { get; set; }
    }
}
