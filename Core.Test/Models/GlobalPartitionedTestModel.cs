#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Partitions;

#endregion

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("GlobalTestModel")]
    internal class GlobalPartitionedTestModel : IEntity, IGlobalPartitionStrategy
    {

        #region Properties

        [EntityKey<Guid>("Id")]
        internal Guid Id { get; set; } = default;

        [EntityField<string>("ModelDescription")]
        internal string Description { get; set; } = default!;

        public bool IsRetrievedFromStorage { get; set; } = false;

        #endregion

    }

}
