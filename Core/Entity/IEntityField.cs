namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents an entity field.
    /// </summary>
    public interface IEntityField
    {

        #region Interface properties

        /// <summary>
        /// Gets a value indicating whether this field is a key field.
        /// </summary>
        bool IsKeyField { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Gets the ordinal position of the field.
        /// </summary>
        int OrdinalPosition { get; }

        #endregion

    }

}
