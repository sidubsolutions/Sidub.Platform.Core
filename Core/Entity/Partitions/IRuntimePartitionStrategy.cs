namespace Sidub.Platform.Core.Entity.Partitions
{

    /// <summary>
    /// Represents the runtime partition strategy interface.
    /// </summary>
    public interface IRuntimePartitionStrategy : IPartitionStrategy
    {

        #region Interface properties

        /// <summary>
        /// Gets or sets the partition ID.
        /// </summary>
        string PartitionId { get; set; }

        #endregion

    }

}
