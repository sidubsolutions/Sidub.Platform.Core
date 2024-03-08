#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Serializers.Json.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Provides functionality to serialize and deserialize entities to and from JSON format.
    /// </summary>
    public class EntityJsonSerializer : IEntityJsonSerializer
    {

        #region Member variables

        private readonly JsonSerializerOptions _serializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityJsonSerializer"/> class.
        /// </summary>
        public EntityJsonSerializer()
        {
            _serializerOptions = new JsonSerializerOptions();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Deserializes the JSON data to an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="jsonData">The JSON data to deserialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when the deserialized entity is null.</exception>
        public TEntity Deserialize<TEntity>(byte[] jsonData, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            return JsonSerializer.Deserialize<TEntity>(jsonData, jsonSerializerOptions) ?? throw new Exception("Null entity deserialized.");
        }

        /// <summary>
        /// Deserializes the dictionary representation of JSON data to an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="jsonData">The dictionary representation of JSON data to deserialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when the deserialized entity is null.</exception>
        public TEntity DeserializeDictionary<TEntity>(IDictionary<string, object?> jsonData, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            var serialized = JsonSerializer.SerializeToUtf8Bytes(jsonData, jsonSerializerOptions);
            var result = JsonSerializer.Deserialize<TEntity>(serialized, jsonSerializerOptions) ?? throw new Exception("Null entity deserialized.");

            return result;
        }

        /// <summary>
        /// Deserializes the JSON data to a collection of entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="jsonData">The JSON data to deserialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The deserialized collection of entities.</returns>
        /// <exception cref="Exception">Thrown when the deserialized collection is null.</exception>
        public IEnumerable<TEntity> DeserializeEnumerable<TEntity>(byte[] jsonData, IEntitySerializerOptions options) where TEntity : class, IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            return JsonSerializer.Deserialize<IEnumerable<TEntity>>(jsonData, jsonSerializerOptions) ?? throw new Exception("Null entity deserialized.");
        }

        /// <summary>
        /// Serializes the entity to JSON format.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <param name="additionalFields">Additional fields to include in the serialized JSON.</param>
        /// <returns>The serialized entity in JSON format.</returns>
        public byte[] Serialize<TEntity>(TEntity entity, IEntitySerializerOptions options, IDictionary<string, object?>? additionalFields) where TEntity : IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            if (additionalFields is null)
                return JsonSerializer.SerializeToUtf8Bytes(entity, jsonSerializerOptions);
            else
            {
                var dictionary = SerializeDictionary(entity, options);
                foreach (var i in additionalFields)
                    dictionary.Add(i.Key, i.Value);

                return JsonSerializer.SerializeToUtf8Bytes(dictionary, jsonSerializerOptions);
            }
        }

        /// <summary>
        /// Serializes the collection of entities to JSON format.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The collection of entities to serialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The serialized collection of entities in JSON format.</returns>
        public byte[] SerializeEnumerable<TEntity>(IEnumerable<TEntity> entities, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            return JsonSerializer.SerializeToUtf8Bytes(entities, jsonSerializerOptions);
        }

        /// <summary>
        /// Serializes the entity to a dictionary representation of JSON format.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The serialized entity in a dictionary representation of JSON format.</returns>
        public Dictionary<string, object?> SerializeDictionary<TEntity>(TEntity entity, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var jsonSerializerOptions = GetJsonSerializerOptions(options);

            ReadOnlySpan<byte> serialized = Serialize(entity, options, null);
            var result = JsonSerializer.Deserialize<Dictionary<string, object?>>(serialized, jsonSerializerOptions) ?? throw new NotImplementedException("TODO - serialize dict produced null dict.");

            return result;
        }

        /// <summary>
        /// Determines whether the specified entity type is handled by this serializer for the given serialization language.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns><c>true</c> if the entity type is handled; otherwise, <c>false</c>.</returns>
        public virtual bool IsHandled<TEntity>(SerializationLanguageType serializationLanguage) where TEntity : IEntity
        {
            if (serializationLanguage != SerializationLanguageType.Json)
                return false;

            return typeof(TEntity).GetInterfaces().Any(x => x == typeof(IEntity));
        }

        // TODO - performance, cache this stuff!!!
        /// <summary>
        /// Gets the JSON serializer options based on the entity serializer options.
        /// </summary>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The JSON serializer options.</returns>
        /// <exception cref="ArgumentException">Thrown when an unhandled type of entity serializer options is encountered.</exception>
        public JsonSerializerOptions GetJsonSerializerOptions(IEntitySerializerOptions options)
        {
            var typed = options as JsonEntitySerializerOptions ?? throw new ArgumentException("Unhandled type encountered.", nameof(options));

            var jsonSerializerOptions = new JsonSerializerOptions(_serializerOptions);

            var attributeEntityConverter = new AttributeEntityConverterFactory(typed);
            var attributeEntityRecordRelationConverter = new AttributeEntityReferenceConverterFactory(typed);
            var attributeEntityEnumerableRelationConverter = new AttributeEntityReferenceListConverterFactory(typed);

            var dictionaryConverter = new DictionaryStringObjectJsonConverter();
            var typeDiscriminatorConverter = new TypeDiscriminatorJsonConverter();
            var enumConverter = new JsonStringEnumConverter(allowIntegerValues: false);
            var dateTimeConverter = new DateTimeJsonConverter();
            var dateTimeNullableConverter = new DateTimeNullableJsonConverter();

            jsonSerializerOptions.Converters.Add(attributeEntityConverter);
            jsonSerializerOptions.Converters.Add(attributeEntityRecordRelationConverter);
            jsonSerializerOptions.Converters.Add(attributeEntityEnumerableRelationConverter);
            jsonSerializerOptions.Converters.Add(dictionaryConverter);
            jsonSerializerOptions.Converters.Add(typeDiscriminatorConverter);
            jsonSerializerOptions.Converters.Add(enumConverter);
            jsonSerializerOptions.Converters.Add(dateTimeConverter);
            jsonSerializerOptions.Converters.Add(dateTimeNullableConverter);

            foreach (var i in typed.Converters)
                jsonSerializerOptions.Converters.Add(i);

            return jsonSerializerOptions;
        }

        #endregion

    }
}
