namespace Sidub.Platform.Core.Serializers.Xml
{

    /// <summary>
    /// Represents the options for XML entity serialization.
    /// </summary>
    public class XmlEntitySerializerOptions : IEntitySerializerOptions
    {

        #region Public properties

        /// <summary>
        /// Gets the serialization language type.
        /// </summary>
        public SerializationLanguageType Language => SerializationLanguageType.Xml;

        /// <summary>
        /// Gets or sets the field serialization type.
        /// </summary>
        public EntityFieldType FieldSerialization { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include type information during serialization.
        /// </summary>
        public bool IncludeTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to serialize relationships.
        /// </summary>
        public bool SerializeRelationships { get; set; } = false;

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the <see cref="XmlEntitySerializerOptions"/> object.
        /// </summary>
        /// <returns>A new instance of the <see cref="XmlEntitySerializerOptions"/> class.</returns>
        public IEntitySerializerOptions Clone()
        {
            var result = new XmlEntitySerializerOptions();

            result.IncludeTypeInfo = IncludeTypeInfo;
            result.FieldSerialization = FieldSerialization;

            return result;
        }

        #endregion

    }
}
