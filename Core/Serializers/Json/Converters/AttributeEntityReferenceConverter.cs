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
using Sidub.Platform.Core.Entity.Relations;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts an <see cref="IEntityReference"/> to and from JSON.
    /// </summary>
    public class AttributeEntityReferenceConverter : JsonConverterFactory
    {

        #region Member variables

        private readonly JsonEntitySerializerOptions _entitySerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityReferenceSerializer"/> class.
        /// </summary>
        /// <param name="serializerOptions">The <see cref="JsonEntitySerializerOptions"/> to use.</param>
        public AttributeEntityReferenceConverter(JsonEntitySerializerOptions serializerOptions)
        {
            _entitySerializerOptions = serializerOptions;
        }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            // we need type information available, thus we can only operate against the generic interface...
            var valid = typeof(IEntityReference).IsAssignableFrom(typeToConvert);

            return valid;
        }

        /// <inheritdoc/>
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var args = typeToConvert.GetGenericArguments();
            var tRelated = args[0];

            Type converterType = typeof(Converter<>).MakeGenericType(new[] { tRelated });
            var converter = Activator.CreateInstance(converterType, _entitySerializerOptions.Clone());

            return (JsonConverter?)converter;
        }

        #endregion

        /// <summary>
        /// Converts an <see cref="IEntityReference"/> to and from JSON.
        /// </summary>
        /// <typeparam name="TRelated">The type of the related entity.</typeparam>
        private class Converter<TRelated> : JsonConverter<IEntityReference>
            where TRelated : IEntity
        {


            #region Member variables

            private readonly JsonEntitySerializerOptions _serializerOptions;

            #endregion

            #region Public properties

            public override bool HandleNull => true;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Converter{TRelated}"/> class.
            /// </summary>
            /// <param name="serializerOptions">The <see cref="JsonEntitySerializerOptions"/> to use.</param>
            public Converter(JsonEntitySerializerOptions serializerOptions)
            {
                _serializerOptions = serializerOptions;
            }

            #endregion

            #region Public methods

            /// <inheritdoc/>
            public override IEntityReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var relatedEntityKeys = EntityTypeHelper.GetEntityFields<TRelated>(EntityFieldType.Keys);

                // begin reading through the data until we finish parsing this relationship; we're dealing with a record relationship, so no need
                //  to deal with enumerables here...
                var relationshipBuilder = new EntityRelationshipBuilder<TRelated>();
                int depth = 0;
                var firstIteration = true;

                // note we do not want to trigger a read on the first iteration since the reader is already
                //  positioned as expected...
                while (firstIteration || reader.Read())
                {
                    // clear the firstIteration flag so further reads are triggered...
                    firstIteration = false;

                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // increment depth...
                        depth++;
                    }
                    else if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        // decrement depth...
                        depth--;

                        // end object at depth 0 means we're at the ending of the TRelated object, end / return...
                        if (depth == 0)
                        {
                            break;
                        }
                    }

                    // handle property names...
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString()
                            ?? throw new Exception("Failed to get property name from related entity data.");

                        var relatedKey = relatedEntityKeys.SingleOrDefault(x => x.FieldName == propertyName);

                        if (relatedKey is not null)
                        {
                            // the property name matches a key field in our related entity, iterate the reader forward once
                            //  so we can grab the relation value...
                            if (!reader.Read())
                                throw new Exception("Unexpected end of data; failed to move reader forward during relation key reading.");

                            var relatedValue = JsonSerializer.Deserialize(ref reader, relatedKey.FieldType, options)
                                ?? throw new Exception("Failed to read relation key value.");

                            relationshipBuilder.AddKeyValueToCurrentReference(relatedKey, relatedValue);
                        }
                        else if (propertyName == TypeDiscriminatorEntityField.Instance.FieldName)
                        {
                            // type discriminator exists...
                            if (!reader.Read())
                                throw new Exception("Unexpected end of data; failed to move reader forward during relation key reading.");

                            var typeDiscriminator = reader.GetString()
                                ?? throw new Exception("Failed to get type discriminator information.");

                            relationshipBuilder.SetConcreteType(typeDiscriminator);
                        }
                    }
                }

                // at this point, the relationship builder should be populated with the information that was available from data...
                var entityReference = relationshipBuilder.CreateReference();

                return entityReference;
            }

            /// <inheritdoc/>
            public override void Write(Utf8JsonWriter writer, IEntityReference value, JsonSerializerOptions options)
            {
                // note, an alternative thought might be to pass the entity from the reference into the serialization layer, requesting
                //  only key serialization; however there is no guarantee the entity has actually been populated... that would require
                //  an asynchronous context to execute within, which we do not have here (not to mention performance impact)... as such
                //  we will serialize the keys directly from the reference...
                writer.WriteStartObject();

                var isAbstract = EntityTypeHelper.IsEntityAbstract<TRelated>();

                if (isAbstract)
                {
                    // we need to write the type discriminator...
                    var typeDiscriminator = value.ConcreteType;
                    writer.WriteString(TypeDiscriminatorEntityField.Instance.FieldName, typeDiscriminator?.ToString());
                }

                foreach (var key in value.EntityKeys)
                {
                    // assuming we have a key match...
                    writer.WritePropertyName(key.Key.FieldName);
                    JsonSerializer.Serialize(writer, key.Value, key.Key.FieldType, options);
                }

                writer.WriteEndObject();
            }

            #endregion

        }
    }
}
