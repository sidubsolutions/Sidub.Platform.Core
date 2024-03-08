#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Serializers;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Entity serialization service provides functionality for serializing entities to and
    /// from JSON byte arrays. Additionally, it allows the retrieval of a JsonSerializerOptions
    /// object, provided a given entity; this allows the converters to be used externally.
    /// 
    /// The primary method of entity serialization is using attributes. Assuming all entities
    /// being serialized implement the same serialization method, only one JsonSerializerOptions
    /// object will be needed which includes the converters for the given serialization implementation.
    /// 
    /// If serializing entities of varying serialization implementation (i.e., attribute-based and map-based),
    /// be sure to retrieve all appropriate option classes to collect all required converters.
    /// </summary>
    public interface IEntitySerializerService
    {

        #region Interface methods

        /// <summary>
        /// Serializes an entity into a JSON representation, returning it as a byte array.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="entity">The instance of the entity to serialize.</param>
        /// <param name="serializerOptions">The options for serialization.</param>
        /// <param name="additionalFields">Additional fields to include in the serialized JSON.</param>
        /// <returns>The JSON representation of the entity in byte array format.</returns>
        byte[] Serialize<TEntity>(TEntity entity, IEntitySerializerOptions serializerOptions, IDictionary<string, object?>? additionalFields = null) where TEntity : IEntity;

        /// <summary>
        /// Serializes multiple entities into a JSON representation, returning it as a byte array.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="entities">The enumerable of entities to serialize.</param>
        /// <param name="serializerOptions">The options for serialization.</param>
        /// <returns>The JSON representation in byte array format.</returns>
        byte[] SerializeEnumerable<TEntity>(IEnumerable<TEntity> entities, IEntitySerializerOptions serializerOptions) where TEntity : IEntity;

        /// <summary>
        /// Deserializes a JSON byte representation of an entity into a typed entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="data">The JSON representation of the entity in byte array format.</param>
        /// <param name="serializerOptions">The options for serialization.</param>
        /// <returns>An instance of the deserialized entity.</returns>
        TEntity Deserialize<TEntity>(byte[] data, IEntitySerializerOptions serializerOptions) where TEntity : IEntity;

        /// <summary>
        /// Deserializes a JSON byte representation of an entity array into an enumerable of typed entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="data">The JSON representation of the entities array in byte array format.</param>
        /// <param name="serializerOptions">The options for serialization.</param>
        /// <returns>An enumerable of deserialized entities.</returns>
        IEnumerable<TEntity> DeserializeEnumerable<TEntity>(byte[] data, IEntitySerializerOptions serializerOptions) where TEntity : class, IEntity;

        /// <summary>
        /// Gets the entity serializer for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="serializerOptions">The options for serialization.</param>
        /// <returns>The entity serializer.</returns>
        IEntitySerializer GetEntitySerializer<TEntity>(IEntitySerializerOptions serializerOptions) where TEntity : IEntity;

        /// <summary>
        /// Serializes an entity into a dictionary representation.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="entity">The instance of the entity to serialize.</param>
        /// <param name="options">The options for serialization.</param>
        /// <returns>The dictionary representation of the entity.</returns>
        Dictionary<string, object?> SerializeDictionary<TEntity>(TEntity entity, IEntitySerializerOptions options) where TEntity : class, IEntity;

        /// <summary>
        /// Deserializes a dictionary representation of an entity into a typed entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="jsonData">The dictionary representation of the entity.</param>
        /// <param name="options">The options for serialization.</param>
        /// <returns>An instance of the deserialized entity.</returns>
        TEntity DeserializeDictionary<TEntity>(IDictionary<string, object?> jsonData, IEntitySerializerOptions options) where TEntity : IEntity;

        #endregion

    }
}
