using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("SpecializedWidget")]
    public interface IWidgetSpecialization : IEntity
    {

        [EntityField<string>("BaseInterfaceProperty")]
        public string BaseInterfaceProperty { get; set; }

    }

    public interface IWidgetSpecialization<T> : IWidgetSpecialization
    {

        [EntityField<string>("SpecializedInterfaceProperty")]
        public string SpecializedInterfaceProperty { get; set; }

    }
}
