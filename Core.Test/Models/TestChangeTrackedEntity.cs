#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity.ChangeTracking;

#endregion

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel")]
    public class TestChangeTrackedEntity : EntityChangeTrackingBase
    {

        #region Properties

        [EntityKey<Guid>("Id")]
        public Guid Id { get; set; } = default;

        [EntityField<string>("ModelDescription")]
        public string Description { get; set; } = default!;

        [EntityField<int>("Counter")]
        public int Counter { get; set; } = default;

        #endregion

    }

}
