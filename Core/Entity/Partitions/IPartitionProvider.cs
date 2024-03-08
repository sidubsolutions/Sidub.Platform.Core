#region Imports

using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Entity.Partitions
{

    /// <summary>
    /// Represents a partition provider that is responsible for generating partition values for entities.
    /// </summary>
    public interface IPartitionProvider
    {

        #region Interface methods

        /// <summary>
        /// Gets the partition value for the specified entity.
        /// </summary>
        /// <param name="entity">The entity for which to get the partition value.</param>
        /// <returns>The partition value for the entity.</returns>
        string? GetPartitionValue(IEntity entity);

        /// <summary>
        /// Gets the partition value for the specified entity reference.
        /// </summary>
        /// <param name="entityReference">The entity reference for which to get the partition value.</param>
        /// <returns>The partition value for the entity reference.</returns>
        string? GetPartitionValue(IEntityReference entityReference);

        #endregion

    }

}
