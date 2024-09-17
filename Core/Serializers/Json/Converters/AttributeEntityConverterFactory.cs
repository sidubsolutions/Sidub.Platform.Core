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
using Sidub.Platform.Core.Entity.Relations;
using Sidub.Platform.Core.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Factory class for creating attribute-based entity converters for JSON serialization.
    /// </summary>
    public class AttributeEntityConverterFactory : JsonConverterFactory
    {

        #region Member variables

        private readonly JsonEntitySerializerOptions _entitySerializerOptions;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the JSON entity serializer options used by the converter factory.
        /// </summary>
        public JsonEntitySerializerOptions EntitySerializerOptions { get => _entitySerializerOptions; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityConverterFactory"/> class.
        /// </summary>
        /// <param name="serializerOptions">The JSON entity serializer options.</param>
        public AttributeEntityConverterFactory(JsonEntitySerializerOptions serializerOptions)
        {
            _entitySerializerOptions = serializerOptions;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the converter factory can convert the specified type.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <returns><c>true</c> if the converter factory can convert the specified type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.GetInterface(nameof(IEntity)) is null)
                return false;

            var result = EntityTypeHelper.IsEntity(typeToConvert);

            return result;
        }

        /// <summary>
        /// Creates a converter for the specified type.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The created converter.</returns>
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(AttributeEntityConverter<>).MakeGenericType(new[] { typeToConvert });
            var converter = Activator.CreateInstance(converterType, EntitySerializerOptions.Clone());

            return (JsonConverter?)converter;
        }

        #endregion

        /// <summary>
        /// Converter class for attribute-based entity serialization.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        public class AttributeEntityConverter<TEntity> : JsonConverter<TEntity> where TEntity : IEntity
        {

            #region Member variables

            private readonly JsonEntitySerializerOptions _serializerOptions;

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the JSON entity serializer options used by the converter.
            /// </summary>
            public JsonEntitySerializerOptions SerializerOptions { get => _serializerOptions; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AttributeEntityConverter{TEntity}"/> class.
            /// </summary>
            /// <param name="serializerOptions">The JSON entity serializer options.</param>
            public AttributeEntityConverter(JsonEntitySerializerOptions serializerOptions)
            {
                _serializerOptions = serializerOptions;
            }

            #endregion

            #region Public methods

            /// <inheritdoc/>
            public override TEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                TEntity result;

                if (EntityTypeHelper.IsEntityAbstract<TEntity>())
                {
                    // if our target type is an interface or abstract class, we need to derive type information... note
                    //  that the reader is a struct, so we're essentially cloning by assigning to a variable...
                    var readerClone = reader;
                    string? typeDiscriminatorFieldValue = null;

                    // read through the JSON to find the type...
                    while (readerClone.Read())
                    {
                        // look for the type discriminator field...
                        if (readerClone.TokenType == JsonTokenType.PropertyName && readerClone.GetString() == TypeDiscriminatorEntityField.Instance.FieldName)
                        {
                            readerClone.Read();
                            typeDiscriminatorFieldValue = readerClone.GetString();
                            break;
                        }

                        // skip nested objects - we're only interested in the root object...
                        if (readerClone.TokenType == JsonTokenType.StartObject)
                        {
                            readerClone.Skip();
                        }
                    }

                    // if we didn't find the type discriminator field, throw an exception...
                    if (string.IsNullOrEmpty(typeDiscriminatorFieldValue) || typeDiscriminatorFieldValue is null)
                        throw new Exception("Error in getting type discriminator information.");

                    var discriminator = TypeDiscriminator.FromString(typeDiscriminatorFieldValue);

                    // get the concrete type from the discriminator...
                    Type concreteType = discriminator.GetDefinedType();
                    result = (TEntity)(Activator.CreateInstance(concreteType) ?? throw new Exception("Null encountered when creating instance of TEntity."));
                }
                else
                {
                    // if our target type is not an interface or abstract class, we can just create an instance of it...
                    result = (TEntity)(Activator.CreateInstance(typeof(TEntity)) ?? throw new Exception("Null encountered when creating instance of TEntity."));
                }

                // suppress change tracking while deserializing...
                IEntityChangeTracking? changeTracking = result as IEntityChangeTracking;

                if (changeTracking is not null)
                    changeTracking.SuppressChangeTracking = true;

                // retrieve fields
                IEnumerable<IEntityField> entityFields;

                // note, be sure not to use the generic methods available w/ TEntity as TEntity in this context may be an interface or abstract while
                //  we want the concrete runtime type...
                entityFields = EntityTypeHelper.GetEntityFields(result, _serializerOptions.FieldSerialization);
                var relations = EntityTypeHelper.GetEntityRelations<TEntity>();

                int objectDepth = 0;

                // read through the JSON...
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // entering an object, increment depth...
                        objectDepth++;
                    }
                    else if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        // exiting an object, decrement depth...
                        objectDepth--;

                        if (objectDepth < 0)
                        {
                            // exiting the root object, therefore return result...
                            if (changeTracking is not null)
                                changeTracking.SuppressChangeTracking = false;

                            return result;
                        }
                    }
                    else if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        // retrieve property / field / relation infos...
                        var propertyName = reader.GetString() ?? throw new JsonException("Null string produced.");
                        var entityField = entityFields.SingleOrDefault(x => x.FieldName == propertyName);
                        var relation = entityField is not null ? null : relations.SingleOrDefault(x => x.Name == propertyName);

                        if (entityField is not null)
                        {
                            // if the property matches an entity field, assign it...
                            var targetType = entityField.FieldType;
                            var fieldSerializer = EntityTypeHelper.GetEntityFieldConverter(result, entityField);

                            JsonSerializerOptions? innerOptions = null;

                            if (fieldSerializer is not null)
                                innerOptions = new JsonSerializerOptions(options).With(x => x.Converters.Add(fieldSerializer));

                            // deserialize the field...
                            var deserialized = JsonSerializer.Deserialize(ref reader, targetType, innerOptions ?? options);

                            // if the deserialized value is null and the target type is a non-nullable value type, throw an exception...
                            if (deserialized is null && targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null)
                                throw new Exception($"JsonSerializer deserialized to null for non-nullable entity field '{entityField.FieldName}'.");

                            EntityTypeHelper.SetEntityFieldValue(result, entityField, deserialized);
                        }
                        else if (relation is not null)
                        {
                            // if the property matches an entity relation, assign it...
                            if (relation.IsEnumerableRelation)
                            {
                                // handle enumerable relations...
                                var referenceType = typeof(EntityReferenceList<>).MakeGenericType(relation.RelatedType);
                                var referenceObject = JsonSerializer.Deserialize(ref reader, referenceType, options);

                                if (referenceObject is null && Nullable.GetUnderlyingType(relation.RelatedType) is null)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReference.");

                                if (referenceObject is not IEntityReferenceList reference)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReference.");

                                EntityTypeHelper.SetEntityRelationReference(result, relation, reference);
                            }
                            else
                            {
                                // handle singular relations...
                                var referenceType = typeof(EntityReference<>).MakeGenericType(relation.RelatedType);
                                var referenceObject = JsonSerializer.Deserialize(ref reader, referenceType, options);

                                if (referenceObject is null && Nullable.GetUnderlyingType(relation.RelatedType) is null)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReference.");

                                if (referenceObject is not IEntityReference reference)
                                    throw new Exception("Failed to deserialize entity record reference as IEntityReference.");

                                EntityTypeHelper.SetEntityRelationReference(result, relation, reference);
                            }
                        }
                    }
                }

                // remove suppression of change tracking...
                if (changeTracking is not null)
                    changeTracking.SuppressChangeTracking = false;

                return result;
            }

            /// <inheritdoc/>
            public override void Write(Utf8JsonWriter writer, TEntity value, JsonSerializerOptions options)
            {
                // retrieve fields
                IEnumerable<IEntityField> entityFields;

                // note, be sure not to use the generic methods available w/ TEntity as TEntity in this context may a polymorphic entity... if
                //  we were to use the generic methods, we'd only get the fields of the polymorphic entity interface, not the concrete entity...
                entityFields = EntityTypeHelper.GetEntityFields(value, _serializerOptions.FieldSerialization);

                writer.WriteStartObject();

                // if the entity is abstract, we need to write the type discriminator...
                if (SerializerOptions.IncludeTypeInfo && EntityTypeHelper.IsEntityAbstract<TEntity>())
                {
                    var isVersioned = EntityTypeHelper.IsEntityVersioned(value);
                    var typeDiscriminator = TypeDiscriminator.From(value, isVersioned);
                    writer.WriteString(TypeDiscriminatorEntityField.Instance.FieldName, typeDiscriminator.ToString());
                }

                var fieldComparer = new EntityFieldComparer();

                // write fields...
                foreach (var i in entityFields)
                {
                    // ensure the field is not excluded...
                    if (_serializerOptions.ExcludedFields.Any(x => fieldComparer.Equals(i, x)))
                        continue;

                    writer.WritePropertyName(i.FieldName);

                    var entityFieldType = i.FieldType;
                    var fieldValue = EntityTypeHelper.GetEntityFieldValue(value, i)!;
                    var fieldSerializer = EntityTypeHelper.GetEntityFieldConverter(value, i);

                    JsonSerializerOptions? optionsWithConverter = null;

                    if (fieldSerializer is not null)
                        optionsWithConverter = new JsonSerializerOptions(options).With(x => x.Converters.Add(fieldSerializer));

                    JsonSerializer.Serialize(writer, fieldValue, entityFieldType, optionsWithConverter ?? options);
                }

                // write relations...
                if (_serializerOptions.SerializeRelationships)
                {
                    var relations = EntityTypeHelper.GetEntityRelations<TEntity>();

                    foreach (var relation in relations)
                    {
                        writer.WritePropertyName(relation.Name);

                        if (relation.IsEnumerableRelation)
                        {
                            // handle enumerable relations...
                            var relationReference = EntityTypeHelper.GetEntityRelationEnumerable(value, relation)
                                ?? throw new Exception("Failed to retrieve relation enumerable reference.");

                            var referenceType = typeof(EntityReferenceList<>).MakeGenericType(relation.RelatedType);

                            var converted = JsonSerializer.Serialize(relationReference, referenceType, options);

                            writer.WriteRawValue(converted);
                        }
                        else
                        {
                            // handle singular relations...
                            var relationReference = EntityTypeHelper.GetEntityRelationRecord(value, relation)
                                ?? throw new Exception("Failed to retrieve relation record reference.");

                            var referenceType = typeof(EntityReference<>).MakeGenericType(relation.RelatedType);

                            var converted = JsonSerializer.Serialize(relationReference, referenceType, options);

                            writer.WriteRawValue(converted);
                        }


                    }
                }

                writer.WriteEndObject();
            }

            #endregion

        }


    }




}
