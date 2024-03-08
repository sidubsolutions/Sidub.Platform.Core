#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Serializers;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Extension methods for the EntitySerializerService.
    /// </summary>
    public static class EntitySerializerServiceExtension
    {

        #region Public static methods

        /// <summary>
        /// Serializes an entity to a byte array using the specified serializer service and options.
        /// </summary>
        /// <typeparam name="TEntitySerializerService">The type of the entity serializer service.</typeparam>
        /// <param name="serializerService">The entity serializer service.</param>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The serialized entity as a byte array.</returns>
        /// <exception cref="Exception">Thrown when the serialize method is not found on the serializer service or when a null serialization result is encountered.</exception>
        public static byte[] Serialize<TEntitySerializerService>(this TEntitySerializerService serializerService, IEntity entity, IEntitySerializerOptions serializerOptions) where TEntitySerializerService : IEntitySerializerService
        {
            var serializeMethod = serializerService.GetType().GetMethod(nameof(IEntitySerializerService.Serialize))?.MakeGenericMethod(entity.GetType())
                ?? throw new Exception("Serialize method not found on serializer service.");

            var result = serializeMethod.Invoke(serializerService, new object?[] { entity, serializerOptions, null }) as byte[]
                ?? throw new Exception("Null serialization result encountered");

            return result;
        }

        /// <summary>
        /// Deserializes an entity from a byte array using the specified serializer service, entity type, and options.
        /// </summary>
        /// <typeparam name="TEntitySerializerService">The type of the entity serializer service.</typeparam>
        /// <param name="serializerService">The entity serializer service.</param>
        /// <param name="entityType">The type of the entity to deserialize.</param>
        /// <param name="data">The byte array containing the serialized entity.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when the deserialize method is not found on the serializer service or when a null deserialization result is encountered.</exception>
        public static IEntity Deserialize<TEntitySerializerService>(this TEntitySerializerService serializerService, Type entityType, byte[] data, IEntitySerializerOptions serializerOptions) where TEntitySerializerService : IEntitySerializerService
        {
            if (entityType.GetInterface(nameof(IEntity)) is null)
                throw new Exception($"Entity type '{entityType.Name}' provided does not implement IEntity.");

            var deserializeMethod = serializerService.GetType().GetMethod(nameof(IEntitySerializerService.Deserialize))?.MakeGenericMethod(entityType)
                ?? throw new Exception("Deserialize method not found on serializer service.");

            var result = deserializeMethod.Invoke(serializerService, new object[] { data, serializerOptions }) as IEntity
                ?? throw new Exception("Null deserialization result encountered");

            return result;
        }

        /// <summary>
        /// Serializes an entity to a dictionary using the specified serializer service and options.
        /// </summary>
        /// <typeparam name="TEntitySerializerService">The type of the entity serializer service.</typeparam>
        /// <param name="serializerService">The entity serializer service.</param>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The serialized entity as a dictionary.</returns>
        /// <exception cref="Exception">Thrown when the serialize method is not found on the serializer service or when a null serialization result is encountered.</exception>
        public static Dictionary<string, object?> SerializeDictionary<TEntitySerializerService>(this TEntitySerializerService serializerService, IEntity entity, IEntitySerializerOptions serializerOptions) where TEntitySerializerService : IEntitySerializerService
        {
            var serializeMethod = serializerService.GetType().GetMethod(nameof(IEntitySerializerService.SerializeDictionary))?.MakeGenericMethod(entity.GetType())
                ?? throw new Exception("SerializeDictionary method not found on serializer service.");

            var result = serializeMethod.Invoke(serializerService, new object[] { entity, serializerOptions }) as Dictionary<string, object?>
                ?? throw new Exception("Null serialization result encountered");

            return result;
        }

        /// <summary>
        /// Deserializes an entity from a dictionary using the specified serializer service, entity type, and options.
        /// </summary>
        /// <typeparam name="TEntitySerializerService">The type of the entity serializer service.</typeparam>
        /// <param name="serializerService">The entity serializer service.</param>
        /// <param name="entityType">The type of the entity to deserialize.</param>
        /// <param name="data">The dictionary containing the serialized entity.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        /// <exception cref="Exception">Thrown when the deserialize method is not found on the serializer service or when a null deserialization result is encountered.</exception>
        public static IEntity DeserializeDictionary<TEntitySerializerService>(this TEntitySerializerService serializerService, Type entityType, IDictionary<string, object?> data, IEntitySerializerOptions serializerOptions) where TEntitySerializerService : IEntitySerializerService
        {
            if (entityType.GetInterface(nameof(IEntity)) is null)
                throw new Exception($"Entity type '{entityType.Name}' provided does not implement IEntity.");

            var deserializeMethod = serializerService.GetType().GetMethod(nameof(IEntitySerializerService.DeserializeDictionary))?.MakeGenericMethod(entityType)
                ?? throw new Exception("DeserializeDictionary method not found on serializer service.");

            var result = deserializeMethod.Invoke(serializerService, new object[] { data, serializerOptions }) as IEntity
                ?? throw new Exception("Null deserialization result encountered");

            return result;
        }

        #endregion

    }

}
