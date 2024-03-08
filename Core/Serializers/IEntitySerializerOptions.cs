namespace Sidub.Platform.Core.Serializers
{

    /// <summary>
    /// Represents the options for an entity serializer.
    /// </summary>
    public interface IEntitySerializerOptions
    {

        #region Interface properties

        /// <summary>
        /// Gets the serialization language type.
        /// </summary>
        SerializationLanguageType Language { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to include type information during serialization.
        /// </summary>
        bool IncludeTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets the field serialization type for the entity.
        /// </summary>
        EntityFieldType FieldSerialization { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to serialize relationships of the entity.
        /// </summary>
        bool SerializeRelationships { get; set; }

        #endregion

        #region Interface methods

        /// <summary>
        /// Creates a deep copy of the entity serializer options.
        /// </summary>
        /// <returns>A new instance of the entity serializer options.</returns>
        IEntitySerializerOptions Clone();

        #endregion

    }

}
