#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Serializers;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Service for serializing and deserializing entities using different serializers.
    /// </summary>
    public class EntitySerializerService : IEntitySerializerService
    {

        #region Member variables

        /// <summary>
        /// The collection of entity serializers.
        /// </summary>
        private readonly IEnumerable<IEntitySerializer> _entitySerializers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySerializerService"/> class.
        /// </summary>
        /// <param name="entitySerializers">The collection of entity serializers.</param>
        public EntitySerializerService(IEnumerable<IEntitySerializer> entitySerializers)
        {
            _entitySerializers = entitySerializers;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Serializes an entity to a byte array.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <param name="additionalFields">The additional fields to include in the serialization.</param>
        /// <returns>The serialized entity as a byte array.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public byte[] Serialize<TEntity>(TEntity entity, IEntitySerializerOptions serializerOptions, IDictionary<string, object?>? additionalFields = null) where TEntity : IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(serializerOptions.Language))
                    return i.Serialize(entity, serializerOptions, additionalFields);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Serializes a collection of entities to a byte array.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="entities">The collection of entities to serialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The serialized entities as a byte array.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public byte[] SerializeEnumerable<TEntity>(IEnumerable<TEntity> entities, IEntitySerializerOptions serializerOptions) where TEntity : IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(serializerOptions.Language))
                    return i.SerializeEnumerable(entities, serializerOptions);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Deserializes a byte array to an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The byte array to deserialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public TEntity Deserialize<TEntity>(byte[] data, IEntitySerializerOptions serializerOptions) where TEntity : IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(serializerOptions.Language))
                    return i.Deserialize<TEntity>(data, serializerOptions);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Deserializes a byte array to a collection of entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="data">The byte array to deserialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The deserialized entities as a collection.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public IEnumerable<TEntity> DeserializeEnumerable<TEntity>(byte[] data, IEntitySerializerOptions serializerOptions) where TEntity : class, IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(serializerOptions.Language))
                    return i.DeserializeEnumerable<TEntity>(data, serializerOptions);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Gets the entity serializer for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The entity serializer.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public IEntitySerializer GetEntitySerializer<TEntity>(IEntitySerializerOptions serializerOptions) where TEntity : IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(serializerOptions.Language))
                    return i;
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Serializes an entity to a dictionary.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The serialized entity as a dictionary.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public Dictionary<string, object?> SerializeDictionary<TEntity>(TEntity entity, IEntitySerializerOptions options) where TEntity : class, IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(options.Language))
                    return i.SerializeDictionary(entity, options);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        /// <summary>
        /// Deserializes a dictionary to an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="jsonData">The dictionary to deserialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when a serializer is not found to handle the entity type.</exception>
        public TEntity DeserializeDictionary<TEntity>(IDictionary<string, object?> jsonData, IEntitySerializerOptions options) where TEntity : IEntity
        {
            foreach (var i in _entitySerializers)
            {
                if (i.IsHandled<TEntity>(options.Language))
                    return i.DeserializeDictionary<TEntity>(jsonData, options);
            }

            throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        }

        //public JsonSerializerOptions GetJsonSerializerOptions<TEntity>(IEntitySerializerOptions serializerOptions) where TEntity : IEntity
        //{
        //    foreach (var i in _entitySerializers)
        //    {
        //        if (i.IsHandled<TEntity>(serializerOptions.Language))
        //            return i.GetJsonSerializerOptions(serializerOptions);
        //    }

        //    throw new Exception($"A serializer was not found to handle entity type '{typeof(TEntity).Name}'.");
        //}

        #endregion

    }

}
