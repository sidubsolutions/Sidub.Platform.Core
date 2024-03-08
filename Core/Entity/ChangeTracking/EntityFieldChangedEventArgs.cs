namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Represents the event arguments for a change in an entity field.
    /// </summary>
    public class EntityFieldChangedEventArgs
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldChangedEventArgs"/> class.
        /// </summary>
        /// <param name="field">The entity field that has changed.</param>
        /// <param name="oldValue">The old value of the entity field.</param>
        /// <param name="newValue">The new value of the entity field.</param>
        public EntityFieldChangedEventArgs(IEntityField field, object? oldValue, object? newValue)
        {
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the entity field that has changed.
        /// </summary>
        public IEntityField Field { get; }

        /// <summary>
        /// Gets the old value of the entity field.
        /// </summary>
        public object? OldValue { get; }

        /// <summary>
        /// Gets the new value of the entity field.
        /// </summary>
        public object? NewValue { get; }

        #endregion

    }
}
