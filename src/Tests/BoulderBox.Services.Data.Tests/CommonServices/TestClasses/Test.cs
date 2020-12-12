using System;

using BoulderBox.Data.Common.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Services.Data.Tests.CommonServices.TestClasses
{
    public class Test : IDeletableEntity, IMapTo<TestViewModel>, IMapFrom<TestViewModel>
    {
        public Test(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }

        public string Name { get; set; }

        public int Count { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
