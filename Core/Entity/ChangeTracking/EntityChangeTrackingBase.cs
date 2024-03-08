#region Imports

using System.ComponentModel;

#endregion

namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Base class for entity change tracking.
    /// </summary>
    public abstract class EntityChangeTrackingBase : IEntityChangeTracking
    {

        #region Public properties

        /// <summary>
        /// Gets the original values of the entity fields.
        /// </summary>
        public Dictionary<IEntityField, object?> OriginalValues { get; } = new Dictionary<IEntityField, object?>();

        /// <summary>
        /// Gets or sets a value indicating whether change tracking is suppressed for the entity.
        /// </summary>
        public bool SuppressChangeTracking { get; set; } = false;

        /// <summary>
        /// Gets whether the entity has changes.
        /// </summary>
        bool IChangeTracking.IsChanged => OriginalValues.Count > 0;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is retrieved from storage.
        /// </summary>
        public bool IsRetrievedFromStorage { get; set; }

        #endregion

        #region Public events

        /// <summary>
        /// Event raised when an entity field is changed.
        /// </summary>
        public event EventHandler<EntityFieldChangedEventArgs>? EntityFieldChanged;

        /// <summary>
        /// Raises the <see cref="EntityFieldChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the entity field.</typeparam>
        /// <param name="field">The entity field that changed.</param>
        /// <param name="oldValue">The old value of the entity field.</param>
        /// <param name="newValue">The new value of the entity field.</param>
        protected void OnEntityFieldChanged<T>(IEntityField field, T oldValue, T newValue)
        {
            EntityFieldChanged?.Invoke(this, new EntityFieldChangedEventArgs(field, oldValue, newValue));
        }

        #endregion

        #region Explicit IRevertibleChangeTracking

        /// <summary>
        /// Accepts the changes made to the entity.
        /// </summary>
        void IChangeTracking.AcceptChanges()
        {
            OriginalValues.Clear();
        }

        /// <summary>
        /// Rejects the changes made to the entity and reverts back to the original values.
        /// </summary>
        void IRevertibleChangeTracking.RejectChanges()
        {
            foreach (var i in OriginalValues)
                EntityTypeHelper.SetEntityFieldValue(this, i.Key, i.Value);

            OriginalValues.Clear();
        }

        #endregion

    }
}
