#region Imports

using System.ComponentModel;

#endregion

namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Represents an entity that supports change tracking.
    /// </summary>
    public interface IEntityChangeTracking : IRevertibleChangeTracking, INotifyEntityFieldChanged, IEntity
    {

        #region Interface properties

        /// <summary>
        /// Gets the original values of the entity's fields before any changes were made.
        /// </summary>
        Dictionary<IEntityField, object?> OriginalValues { get; }

        /// <summary>
        /// Gets or sets a value indicating whether change tracking is suppressed for the entity.
        /// </summary>
        bool SuppressChangeTracking { get; set; }

        #endregion

    }
}
