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
using Sidub.Platform.Core.Entity.ChangeTracking;
using Sidub.Platform.Core.Serializers.Xml.Converters;
using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml
{

    /// <summary>
    /// Represents a serializer for XML serialization of attribute-based entities.
    /// </summary>
    public class AttributeEntityXmlSerializer : IEntitySerializer
    {

        #region Public methods

        /// <summary>
        /// Deserializes the XML data into an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="xmlData">The XML data to deserialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        public TEntity Deserialize<TEntity>(byte[] xmlData, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using var memoryStream = new MemoryStream(xmlData);
            using var reader = XmlReader.Create(memoryStream, settings);

            var result = Deserialize<TEntity>(reader, options);

            return result;
        }

        /// <summary>
        /// Deserializes the XML data into a collection of entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="xmlData">The XML data to deserialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized collection of entities.</returns>
        public IEnumerable<TEntity> DeserializeEnumerable<TEntity>(byte[] xmlData, IEntitySerializerOptions options) where TEntity : class, IEntity
        {
            var settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using var memoryStream = new MemoryStream(xmlData);
            using var reader = XmlReader.Create(memoryStream, settings);

            var result = new List<TEntity>();
            var entityLabel = EntityTypeHelper.GetEntityName<TEntity>()
                ?? throw new Exception($"Failed to retrieve entity name from entity type '{typeof(TEntity).Name}'.");

            while (reader.Read())
            {
                if (reader.IsStartElement(entityLabel))
                {
                    var entity = Deserialize<TEntity>(reader, options);
                    result.Add(entity);
                }
            }

            return result;
        }

        /// <summary>
        /// Serializes the entity into XML data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <param name="additionalFields">Additional fields to include in the serialization.</param>
        /// <returns>The serialized XML data.</returns>
        public byte[] Serialize<TEntity>(TEntity entity, IEntitySerializerOptions options, IDictionary<string, object?>? additionalFields) where TEntity : IEntity
        {
            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment
            };

            if (options is not XmlEntitySerializerOptions xmlOptions)
                throw new Exception("Invalid serializer options.");

            using var memoryStream = new MemoryStream();
            using var writer = XmlWriter.Create(memoryStream, settings);

            var entityLabel = EntityTypeHelper.GetEntityName<TEntity>()
                ?? throw new Exception($"Failed to retrieve entity name from entity type '{typeof(TEntity).Name}'.");

            writer.WriteStartElement(entityLabel);

            // retrieve fields
            IEnumerable<IEntityField> entityFields;

            // note, be sure not to use the generic methods available w/ TEntity as TEntity in this context may be an interface or abstract while
            //  we want the concrete runtime type...
            entityFields = EntityTypeHelper.GetEntityFields(entity, options.FieldSerialization);

            foreach (var i in entityFields)
            {
                writer.WriteStartElement(i.FieldName);
                var fieldValue = EntityTypeHelper.GetEntityFieldValue(entity, i);
                string? fieldString = null;
                // todo - elaborate!
                fieldString = XmlValueSerializer.Write(fieldValue);
                writer.WriteValue(fieldString ?? string.Empty);
                writer.WriteEndElement();
            }

            // write relations...
            if (xmlOptions.SerializeRelationships)
            {
                var relations = EntityTypeHelper.GetEntityRelations<TEntity>();

                foreach (var relation in relations)
                {
                    //writer.WriteStartElement(relation.Name);

                    if (relation.IsEnumerableRelation)
                    {
                        // handle enumerable relations...
                        var relationReference = EntityTypeHelper.GetEntityRelationEnumerable(entity, relation)
                            ?? throw new Exception("Failed to retrieve relation enumerable reference.");

                        var relationBuilder = AttributeEntityReferenceListConverter.Create(relation.RelatedType);
                        writer.WriteStartElement(relation.Name);
                        relationBuilder.Write(writer, relationReference, xmlOptions);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        // handle singular relations...
                        var relationReference = EntityTypeHelper.GetEntityRelationRecord(entity, relation)
                            ?? throw new Exception("Failed to retrieve relation record reference.");

                        var relationBuilder = AttributeEntityReferenceConverter.Create(relation.RelatedType);
                        writer.WriteStartElement(relation.Name);
                        relationBuilder.Write(writer, relationReference, xmlOptions);
                        writer.WriteEndElement();
                    }

                    //writer.WriteEndElement();
                }

            }

            // append the additional fields...
            if (additionalFields is not null)
            {
                foreach (var i in additionalFields)
                {
                    writer.WriteStartElement(i.Key);

                    if (i.Value is not null)
                        writer.WriteValue(i.Value);

                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Serializes the collection of entities into XML data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="entities">The collection of entities to serialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The serialized XML data.</returns>
        public byte[] SerializeEnumerable<TEntity>(IEnumerable<TEntity> entities, IEntitySerializerOptions options) where TEntity : IEntity
        {
            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment
            };

            using var memoryStream = new MemoryStream();
            using var writer = XmlWriter.Create(memoryStream, settings);

            var entityLabel = EntityTypeHelper.GetEntityName<TEntity>()
                ?? throw new Exception($"Failed to retrieve entity name from entity type '{typeof(TEntity).Name}'.");

            foreach (var entity in entities)
            {
                var serialized = Serialize(entity, options, null);
                var serializedString = System.Text.Encoding.UTF8.GetString(serialized);
                writer.WriteRaw(serializedString);
            }

            writer.Flush();
            writer.Close();

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Determines if the serializer can handle serialization of entities of type <typeparamref name="TEntity"/> in the specified serialization language.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns><c>true</c> if the serializer can handle the serialization, otherwise <c>false</c>.</returns>
        public bool IsHandled<TEntity>(SerializationLanguageType serializationLanguage) where TEntity : IEntity
        {
            if (serializationLanguage != SerializationLanguageType.Xml)
                return false;

            var result = EntityTypeHelper.IsEntity<TEntity>();

            return result;
        }

        /// <summary>
        /// Serializes the entity into a dictionary of field names and values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The serialized dictionary of field names and values.</returns>
        public Dictionary<string, object?> SerializeDictionary<TEntity>(TEntity entity, IEntitySerializerOptions options) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes the dictionary of field names and values into an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="xmlData">The dictionary of field names and values to deserialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        public TEntity DeserializeDictionary<TEntity>(IDictionary<string, object?> xmlData, IEntitySerializerOptions options) where TEntity : IEntity
        {
            TEntity result;
            IEnumerable<IEntityField> entityFields;

            var entityLabel = EntityTypeHelper.GetEntityName<TEntity>()
                ?? throw new Exception($"Failed to retrieve entity name from entity type '{typeof(TEntity).Name}'.");

            if (xmlData.ContainsKey(TypeDiscriminatorEntityField.Instance.FieldName))
            {
                Type concreteType = typeof(TEntity);
                var typeDiscriminatorFieldValue = xmlData[TypeDiscriminatorEntityField.Instance.FieldName]?.ToString()
                    ?? throw new Exception("Type discriminator found with invalid or blank value.");

                var discriminator = TypeDiscriminator.FromString(typeDiscriminatorFieldValue);
                concreteType = discriminator.GetDefinedType()
                    ?? throw new Exception("Error initializing type discriminator information.");

                result = (TEntity)Activator.CreateInstance(concreteType)
                    ?? throw new Exception("Null encountered when creating instance of TEntity.");
                entityFields = EntityTypeHelper.GetEntityFields(discriminator, options.FieldSerialization);
            }
            else
            {
                result = (TEntity)Activator.CreateInstance(typeof(TEntity))
                    ?? throw new Exception("Null encountered when creating instance of TEntity.");

                entityFields = EntityTypeHelper.GetEntityFields<TEntity>(options.FieldSerialization);
            }

            foreach (var kvp in xmlData)
            {
                if (kvp.Key == TypeDiscriminatorEntityField.Instance.FieldName)
                    continue;

                var fieldName = kvp.Key;
                var fieldValue = kvp.Value;

                var entityField = entityFields.SingleOrDefault(x => x.FieldName == fieldName);

                if (entityField is not null)
                {
                    EntityTypeHelper.SetEntityFieldValue(result, entityField, fieldValue);
                }
                else
                {
                    throw new Exception($"Field '{fieldName}' does not exist in entity '{entityLabel}'.");
                }
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Deserializes the XML data into an entity of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The XML reader containing the XML data to deserialize.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized entity.</returns>
        private static TEntity Deserialize<TEntity>(XmlReader reader, IEntitySerializerOptions options) where TEntity : IEntity
        {

            TEntity result;

            if (options is not XmlEntitySerializerOptions xmlOptions)
                throw new Exception("Invalid serializer options.");

            if (typeof(TEntity).IsInterface || typeof(TEntity).IsAbstract)
            {
                // if our target type is an interface or abstract class, we need to derive type information...
                var settings = new XmlReaderSettings()
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                };

                var readerClone = XmlReader.Create(reader, settings);
                string? typeDiscriminatorFieldValue = null;

                while (readerClone.Read())
                {
                    if (readerClone.NodeType == XmlNodeType.Element && readerClone.Name == TypeDiscriminatorEntityField.Instance.FieldName)
                    {
                        typeDiscriminatorFieldValue = readerClone.ReadElementContentAsString();
                        break;
                    }

                    // skip nested objects - we're only interested in the root object...
                    if (readerClone.NodeType == XmlNodeType.Element)
                    {
                        readerClone.Skip();
                    }
                }

                if (string.IsNullOrEmpty(typeDiscriminatorFieldValue) || typeDiscriminatorFieldValue is null)
                    throw new Exception("Error in getting type discriminator information.");

                var discriminator = TypeDiscriminator.FromString(typeDiscriminatorFieldValue);

                Type concreteType = discriminator.GetDefinedType() ?? throw new Exception("Error initializing type discriminator information.");
                result = (TEntity)(Activator.CreateInstance(concreteType) ?? throw new Exception("Null encountered when creating instance of TEntity."));
            }
            else
            {
                result = (TEntity)(Activator.CreateInstance(typeof(TEntity)) ?? throw new Exception("Null encountered when creating instance of TEntity."));
            }

            IEntityChangeTracking? changeTracking = result as IEntityChangeTracking;

            if (changeTracking is not null)
                changeTracking.SuppressChangeTracking = true;

            // retrieve fields
            IEnumerable<IEntityField> entityFields;

            // note, be sure not to use the generic methods available w/ TEntity as TEntity in this context may be an interface or abstract while
            //  we want the concrete runtime type...
            entityFields = EntityTypeHelper.GetEntityFields(result, options.FieldSerialization);

            var relations = EntityTypeHelper.GetEntityRelations<TEntity>();

            int objectDepth = 0;

            bool skipRead = false;
            IXmlNamespaceResolver resolver = new XmlNamespaceManager(new NameTable());

            while (skipRead || reader.Read())
            {
                skipRead = false;

                if (reader.NodeType == XmlNodeType.Element)
                {
                    // only increment depth when the element has content...
                    if (!reader.IsEmptyElement)
                        objectDepth++;

                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    objectDepth--;

                    if (objectDepth == 0)
                    {
                        // exiting the root object, therefore return result...
                        if (changeTracking is not null)
                            changeTracking.SuppressChangeTracking = false;

                        return result;
                    }
                }


                if (objectDepth == 1)
                {
                    // object depth of one means we're at the root element... 
                }
                else
                {

                    // object depth of zero means root entity deserialization...
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        // retrieve property / field / relation infos...
                        var propertyName = reader.Name ?? throw new XmlException("Null string produced.");
                        var entityField = entityFields.SingleOrDefault(x => x.FieldName == propertyName);
                        var relation = entityField is not null ? null : relations.SingleOrDefault(x => x.Name == propertyName);

                        if (entityField is not null)
                        {
                            // base deserialization...
                            if (objectDepth > 2)
                                throw new Exception("Nested objects not supported.");

                            // if the property matches an entity field, assign it...
                            var targetType = entityField.FieldType;

                            //if (fieldSerializer is not null)
                            //    innerOptions = new XmlEntitySerializerOptions(options).With(x => x.Converters.Add(fieldSerializer));
                            //var serializer = new XmlSerializer(targetType);

                            var deserialized = reader.ReadElementContentAs(targetType, resolver);
                            skipRead = true;
                            objectDepth--;

                            if (deserialized is null && targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null)
                                throw new Exception($"XmlSerializer deserialized to null for non-nullable entity field '{entityField.FieldName}'.");

                            EntityTypeHelper.SetEntityFieldValue(result, entityField, deserialized);
                        }
                        else if (relation is not null)
                        {


                            // if the property matches an entity relation, assign it...
                            if (relation.IsEnumerableRelation)
                            {
                                // handle enumerable relations...
                                var builder = AttributeEntityReferenceListConverter.Create(relation.RelatedType);
                                var reference = builder.Read(ref reader, xmlOptions);

                                if (reference is null)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReferenceList.");

                                EntityTypeHelper.SetEntityRelationReference(result, relation, reference);
                            }
                            else
                            {
                                // handle singular relations...
                                var builder = AttributeEntityReferenceConverter.Create(relation.RelatedType);
                                var reference = builder.Read(ref reader, xmlOptions);

                                if (reference is null && Nullable.GetUnderlyingType(relation.RelatedType) is null)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReference.");

                                EntityTypeHelper.SetEntityRelationReference(result, relation, reference);
                            }

                            // note that we need to decrement our depth here, as the relation deserialization will have already
                            //  read the end element...
                            objectDepth--;
                        }
                        else
                        {
                            // skip...
                            reader.Skip();
                            objectDepth--;
                            skipRead = true;
                        }
                    }
                }


            }

            throw new Exception("Unexpected parsing encountered.");
        }

        #endregion

    }
}
