namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Represents an interface for notifying when an entity field has changed.
    /// </summary>
    public interface INotifyEntityFieldChanged
    {

        #region Interface events

        /// <summary>
        /// Event that is raised when an entity field has changed.
        /// </summary>
        event EventHandler<EntityFieldChangedEventArgs>? EntityFieldChanged;

        #endregion

    }

}
