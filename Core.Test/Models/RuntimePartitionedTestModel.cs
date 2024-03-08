#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Partitions;

#endregion

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("PartitionedTestModel")]
    internal class RuntimePartitionedTestModel : IEntity, IRuntimePartitionStrategy
    {

        #region Properties

        [EntityKey<Guid>("Id")]
        internal Guid Id { get; set; } = default;

        [EntityKey<string>("PartitionId")]
        public string PartitionId { get; set; } = default!;

        [EntityField<string>("ModelDescription")]
        internal string Description { get; set; } = default!;

        public bool IsRetrievedFromStorage { get; set; } = false;

        #endregion

    }

}
