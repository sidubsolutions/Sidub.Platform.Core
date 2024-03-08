using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel05")]
    public class TestModel05 : IEntity
    {

        public enum TestModelColorType
        {
            Unknown,
            Yellow,
            Green
        }

        [EntityKey<Guid>("ID")]
        public Guid Id { get; set; }

        [EntityField<DateTime>("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public bool IsRetrievedFromStorage { get; set; }
    }
}
