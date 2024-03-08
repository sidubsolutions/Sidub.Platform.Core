using Sidub.Platform.Core.Attributes;

namespace Sidub.Platform.Core.Test.Models
{

    public class NavigationActionTargetPage : INavigationActionTarget
    {

        public Guid Id { get; set; }

        [EntityField<Uri>("TargetPage")]
        public Uri? TargetPage { get; set; }

        public bool IsRetrievedFromStorage { get; set; }

        public NavigationActionTargetPage()
        {

        }

        public NavigationActionTargetPage(Uri targetPage)
        {
            Id = Guid.NewGuid();
            TargetPage = targetPage;
        }

    }
}
