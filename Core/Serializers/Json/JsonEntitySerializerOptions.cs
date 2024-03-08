#region Imports

using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Represents the options for JSON entity serialization.
    /// </summary>
    public class JsonEntitySerializerOptions : IEntitySerializerOptions
    {

        #region Public properties

        /// <summary>
        /// Gets the serialization language type.
        /// </summary>
        public SerializationLanguageType Language => SerializationLanguageType.Json;

        /// <summary>
        /// Gets or sets a value indicating whether to include type information during serialization.
        /// </summary>
        public bool IncludeTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets the field serialization options.
        /// </summary>
        public EntityFieldType FieldSerialization { get; set; }

        /// <summary>
        /// Gets the list of JSON converters to be used during serialization.
        /// </summary>
        public List<JsonConverter> Converters { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to serialize relationships.
        /// </summary>
        public bool SerializeRelationships { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonEntitySerializerOptions"/> class.
        /// </summary>
        public JsonEntitySerializerOptions()
        {
            IncludeTypeInfo = true;
            FieldSerialization = EntityFieldType.Keys | EntityFieldType.Fields;
            Converters = new List<JsonConverter>();
            SerializeRelationships = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the <see cref="JsonEntitySerializerOptions"/> object.
        /// </summary>
        /// <returns>A new instance of the <see cref="JsonEntitySerializerOptions"/> class with the same property values as the original.</returns>
        public IEntitySerializerOptions Clone()
        {
            var result = new JsonEntitySerializerOptions();

            result.IncludeTypeInfo = IncludeTypeInfo;
            result.FieldSerialization = FieldSerialization;
            result.Converters.AddRange(Converters);
            result.SerializeRelationships = SerializeRelationships;

            return result;
        }

        #endregion

    }

}
