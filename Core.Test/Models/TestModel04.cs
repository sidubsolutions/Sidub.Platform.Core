using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel03")]
    public class TestModel04 : IEntity
    {

        public enum TestModelColorType
        {
            Unknown,
            Yellow,
            Green
        }

        [EntityKey<Guid>("ID")]
        public Guid Id { get; set; }

        [EntityField<Guid?>("Value")]
        public Guid? Value { get; set; }
        public bool IsRetrievedFromStorage { get; set; }
    }
}
