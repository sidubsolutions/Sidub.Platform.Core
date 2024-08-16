#region Imports

using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute used to mark a property as an entity key field.
    /// </summary>
    /// <typeparam name="TValue">The type of the entity key field.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityKeyAttribute<TValue> : EntityFieldAttribute<TValue>, IEntityField
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityKeyAttribute{TValue}"/> class.
        /// </summary>
        /// <param name="name">The name of the entity key field.</param>
        /// <param name="label">The label of the entity key field.</param>
        public EntityKeyAttribute(string name, string? label = null, int ordinalPosition = 0) : base(name, label)
        {
            OrdinalPosition = ordinalPosition;
        }

        #endregion

        #region IEntityField implementation

        /// <summary>
        /// Gets a value indicating whether the field is a key field.
        /// </summary>
        bool IEntityField.IsKeyField => true;

        #endregion
    }

}
