#region Imports


#endregion

using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute that is used to mark a property as an entity field.
    /// </summary>
    /// <typeparam name="TValue">The type of the field value.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityFieldAttribute<TValue> : Attribute, IEntityField
    {

        #region Public properties

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        public string Label { get; }

        public int OrdinalPosition { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldAttribute{TValue}"/> class with the specified field name and label.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="label">The label of the field. If not provided, the field name will be used as the label.</param>
        public EntityFieldAttribute(string name, string? label = null)
        {
            FieldName = name;
            Label = label ?? name;
        }

        #endregion

        #region IEntityField implementation

        /// <summary>
        /// Gets a value indicating whether the field is a key field.
        /// </summary>
        bool IEntityField.IsKeyField => false;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        Type IEntityField.FieldType => typeof(TValue);

        #endregion

    }

}
