using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel03")]
    public class TestModel03 : IEntity
    {

        public enum TestModelColorType
        {
            Unknown,
            Yellow,
            Green
        }

        [EntityKey<Guid>("ID")]
        public Guid Id { get; set; }

        [EntityField<string>("Name")]
        public string Name { get; set; } = string.Empty;

        [EntityField<TestModelColorType>("ColorType")]
        public TestModelColorType Color { get; set; }

        public bool IsRetrievedFromStorage { get; set; }

    }
}
