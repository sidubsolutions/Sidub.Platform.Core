using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("NavigationActionTarget")]
    public interface INavigationActionTarget : IEntity
    {

        [EntityKey<Guid>("Id")]
        public Guid Id { get; set; }

    }
}
