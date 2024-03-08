namespace Sidub.Platform.Core.Entity.Partitions
{

    /// <summary>
    /// Represents the global partition strategy.
    /// </summary>
    public static class GlobalPartitionStrategy
    {

        /// <summary>
        /// The partition value for the global partition strategy.
        /// </summary>
        public const string PartitionValue = "global";

    }

    /// <summary>
    /// Represents the interface for the global partition strategy.
    /// </summary>
    public interface IGlobalPartitionStrategy : IPartitionStrategy
    {

    }

}
