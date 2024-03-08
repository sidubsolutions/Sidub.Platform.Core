using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("SpecializedContainer")]
    public class SpecializedContainer : IEntity
    {

        [EntityField<IWidgetSpecialization>("SpecializedWidget")]
        public IWidgetSpecialization<string> SpecializedWidget { get; set; } = new SpecializedWidget()
        {
            BaseInterfaceProperty = "BaseInterfaceProperty",
            SpecializedInterfaceProperty = "SpecializedInterfaceProperty"
        };

        public bool IsRetrievedFromStorage { get; set; }

    }
}
