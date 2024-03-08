namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents an entity in the system.
    /// </summary>
    public interface IEntity
    {

        #region Interface properties

        /// <summary>
        /// Gets or sets a value indicating whether the entity is retrieved from storage.
        /// </summary>
        bool IsRetrievedFromStorage { get; set; }

        #endregion

    }

}
