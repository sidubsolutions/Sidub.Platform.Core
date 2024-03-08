namespace Sidub.Platform.Core.Test.Models
{

    public class SpecializedWidget : IWidgetSpecialization<string>
    {
        public string SpecializedInterfaceProperty { get; set; } = string.Empty;

        public string BaseInterfaceProperty { get; set; } = string.Empty;

        public bool IsRetrievedFromStorage { get; set; }
    }
}
