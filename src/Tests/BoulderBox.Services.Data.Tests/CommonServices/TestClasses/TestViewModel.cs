using BoulderBox.Services.Mapping;

namespace BoulderBox.Services.Data.Tests.CommonServices.TestClasses
{
    public class TestViewModel : IMapTo<Test>, IMapFrom<Test>
    {
        public TestViewModel(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
