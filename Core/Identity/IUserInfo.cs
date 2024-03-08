namespace Sidub.Platform.Core.Identity
{

    /// <summary>
    /// Represents a user information.
    /// </summary>
    public interface IUserInfo
    {

        #region Interface properties

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string Username { get; }

        #endregion

    }

}
