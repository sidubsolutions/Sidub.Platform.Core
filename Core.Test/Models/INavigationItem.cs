using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("NavigationItem")]
    public interface INavigationItem : IEntity
    {

        [EntityKey<string>("Id")]
        public string Id { get; set; }

        [EntityField<string>("Title")]
        public string Title { get; set; }

        [EntityField<string>("Description")]
        public string Description { get; set; }

    }

}
