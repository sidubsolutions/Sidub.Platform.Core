#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Entity partition service provides data partitioning abstractions. Entities
    /// implementing a partitioning strategy will be may be categorized into a string
    /// based partition key. Partition strategies encapsulate the logic to determine
    /// a partition key based on a given entity; this separates entities from partitions
    /// and allows various partition strategies to be applied against a given entity.
    /// </summary>
    public interface IEntityPartitionService
    {

        #region Interface methods

        /// <summary>
        /// Gets the partition value for the specified entity.
        /// </summary>
        /// <param name="entity">The entity for which to get the partition value.</param>
        /// <returns>The partition value as a string.</returns>
        string? GetPartitionValue(IEntity entity);

        /// <summary>
        /// Gets the partition value for the specified entity reference.
        /// </summary>
        /// <param name="entityReference">The entity reference for which to get the partition value.</param>
        /// <returns>The partition value as a string.</returns>
        string? GetPartitionValue(IEntityReference entityReference);

        #endregion

    }
}
