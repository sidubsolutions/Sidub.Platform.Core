using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity.Relations;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("NavigationAction")]
    public class NavigationAction : INavigationItem
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [EntityRecordRelation<INavigationActionTarget>("Target", EntityRelationshipType.Composition)]
        public EntityReference<INavigationActionTarget> Target { get; set; }

        public bool IsRetrievedFromStorage { get; set; }

        public NavigationAction()
        {
            Id = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Target = EntityReference<INavigationActionTarget>.Null;
        }

        public NavigationAction(string id, string title, INavigationActionTarget target)
        {
            Id = id;
            Title = title;
            Description = string.Empty;
            Target = EntityReference.Create(target);
        }



    }

}
