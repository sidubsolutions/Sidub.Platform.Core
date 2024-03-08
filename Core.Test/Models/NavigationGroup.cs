using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity.Relations;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("NavigationGroup")]
    public class NavigationGroup : INavigationItem
    {
        // note, conceptually a composition relationship doesn't make sense here but just for testing... realistically it'd be
        //  an association type...

        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [EntityEnumerableRelation<INavigationItem>("Items", EntityRelationshipType.Composition)]
        public EntityReferenceList<INavigationItem> Items { get; set; } = new EntityReferenceList<INavigationItem>();
        public bool IsRetrievedFromStorage { get; set; }
    }
}
