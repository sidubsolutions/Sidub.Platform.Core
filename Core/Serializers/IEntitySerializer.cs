/*
 * Sidub Platform - Core
 * Copyright (C) 2024 Sidub Inc.
 * All rights reserved.
 *
 * This file is part of Sidub Platform - Core (the "Product").
 *
 * The Product is dual-licensed under:
 * 1. The GNU Affero General Public License version 3 (AGPLv3)
 * 2. Sidub Inc.'s Proprietary Software License Agreement (PSLA)
 *
 * You may choose to use, redistribute, and/or modify the Product under
 * the terms of either license.
 *
 * The Product is provided "AS IS" and "AS AVAILABLE," without any
 * warranties or conditions of any kind, either express or implied, including
 * but not limited to implied warranties or conditions of merchantability and
 * fitness for a particular purpose. See the applicable license for more
 * details.
 *
 * See the LICENSE.txt file for detailed license terms and conditions or
 * visit https://sidub.ca/licensing for a copy of the license texts.
 */

#region Imports

using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Serializers
{

    /// <summary>
    /// Defines a strategy for entity serialization by defining functionality for converting
    /// between entities and a serialized representation.
    /// </summary>
    public interface IEntitySerializer
    {

        #region Interface methods

        /// <summary>
        /// Serializes an entity into a JSON representation, returning as a byte array.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="entity">Instance of entity to serialize.</param>
        /// <param name="serializerOptions">Options for serialization.</param>
        /// <param name="additionalFields">Additional fields to include in the serialized representation.</param>
        /// <returns>JSON representation of entity in byte array format.</returns>
        byte[] Serialize<TEntity>(TEntity entity, IEntitySerializerOptions options, IDictionary<string, object?>? additionalFields) where TEntity : IEntity;

        /// <summary>
        /// Serializes multiple entities into a JSON representation, returning as a byte array.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="entities">Enumerable of entities to serialize.</param>
        /// <param name="serializerOptions">Options for serialization.</param>
        /// <returns>JSON representation in byte array format.</returns>
        byte[] SerializeEnumerable<TEntity>(IEnumerable<TEntity> entities, IEntitySerializerOptions options) where TEntity : IEntity;

        /// <summary>
        /// Deserializes a JSON byte representation of an entity into a typed entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="jsonData">JSON representation of entity in byte array format.</param>
        /// <param name="options">Options for deserialization.</param>
        /// <returns>Instance of deserialized entity.</returns>
        TEntity Deserialize<TEntity>(byte[] jsonData, IEntitySerializerOptions options) where TEntity : IEntity;

        /// <summary>
        /// Deserializes a JSON byte representation of an entity array into an enumerable of typed entities.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="jsonData">JSON representation of entities array in byte array format.</param>
        /// <param name="options">Options for deserialization.</param>
        /// <returns>Enumerable of deserialized entities.</returns>
        IEnumerable<TEntity> DeserializeEnumerable<TEntity>(byte[] jsonData, IEntitySerializerOptions options) where TEntity : class, IEntity;

        /// <summary>
        /// Determines if the given serializer is responsible for the given entity type.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns>True if capable of serializing entity type.</returns>
        bool IsHandled<TEntity>(SerializationLanguageType serializationLanguage) where TEntity : IEntity;

        /// <summary>
        /// Serializes an entity into a dictionary representation, returning as a dictionary.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="entity">Instance of entity to serialize.</param>
        /// <param name="options">Options for serialization.</param>
        /// <returns>Dictionary representation of entity.</returns>
        Dictionary<string, object?> SerializeDictionary<TEntity>(TEntity entity, IEntitySerializerOptions options) where TEntity : IEntity;

        /// <summary>
        /// Deserializes a dictionary representation of an entity into a typed entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="jsonData">Dictionary representation of entity.</param>
        /// <param name="options">Options for deserialization.</param>
        /// <returns>Instance of deserialized entity.</returns>
        TEntity DeserializeDictionary<TEntity>(IDictionary<string, object?> jsonData, IEntitySerializerOptions options) where TEntity : IEntity;

        #endregion

    }

}
