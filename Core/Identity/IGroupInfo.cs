namespace Sidub.Platform.Core.Identity
{

    /// <summary>
    /// Represents a group information.
    /// </summary>
    public interface IGroupInfo
    {

        #region Interface properties

        /// <summary>
        /// Gets the ID of the group.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        string Name { get; }

        #endregion

    }

}
