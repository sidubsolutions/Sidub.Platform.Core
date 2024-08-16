#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts an <see cref="IEntityReferenceList"/> to and from JSON using attribute-based entity references.
    /// </summary>
    public class AttributeEntityReferenceListConverter : JsonConverterFactory
    {

        #region Member variables

        private readonly JsonEntitySerializerOptions _entitySerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityReferenceListConverter"/> class.
        /// </summary>
        /// <param name="serializerOptions">The <see cref="JsonEntitySerializerOptions"/> to use for serialization.</param>
        public AttributeEntityReferenceListConverter(JsonEntitySerializerOptions serializerOptions)
        {
            _entitySerializerOptions = serializerOptions;
        }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            // we need type information available, thus we can only operate against the generic interface...
            var valid = typeof(IEntityReferenceList).IsAssignableFrom(typeToConvert);

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
        /// Converts an <see cref="IEntityReferenceList"/> to and from JSON using attribute-based entity references.
        /// </summary>
        /// <typeparam name="TRelated">The type of the related entity.</typeparam>
        private class Converter<TRelated> : JsonConverter<IEntityReferenceList>
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
            /// <param name="serializerOptions">The <see cref="JsonEntitySerializerOptions"/> to use for serialization.</param>
            public Converter(JsonEntitySerializerOptions serializerOptions)
            {
                _serializerOptions = serializerOptions;
            }

            #endregion

            #region Public methods

            /// <inheritdoc/>
            public override IEntityReferenceList? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // begin reading through the data until we finish parsing this relationship; we're dealing with a record relationship, so no need
                //  to deal with enumerables here...
                var relatedEntities = new EntityReferenceList<TRelated>();
                int depth = 0;
                var firstIteration = true;

                // note we do not want to trigger a read on the first iteration since the reader is already
                //  positioned as expected...
                while (firstIteration || reader.Read())
                {
                    // clear the firstIteration flag so further reads are triggered...
                    firstIteration = false;

                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // increment the depth...
                        depth++;
                    }
                    else if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        // decrement the depth...
                        depth--;

                        // end object at depth 0 means we're at the ending of the TRelated object, end / return...
                        if (depth == 0)
                        {
                            break;
                        }
                    }

                    // for each object contained in the list...
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        var entityReference = JsonSerializer.Deserialize<EntityReference<TRelated>>(ref reader, options)
                            ?? throw new Exception("Null entity reference encountered from deserialization. Unexpected error.");

                        relatedEntities.Add(entityReference);
                    }
                }

                return relatedEntities;
            }

            /// <inheritdoc/>
            public override void Write(Utf8JsonWriter writer, IEntityReferenceList value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();

                foreach (var i in value)
                {
                    // intention is we simply iterate each and call the serializer, the AttributeEntityReferenceConverter should handle...
                    var serialized = JsonSerializer.Serialize(i, typeof(EntityReference<>).MakeGenericType(typeof(TRelated)), options);

                    writer.WriteRawValue(serialized);
                }

                writer.WriteEndArray();
            }

            #endregion

        }
    }




}
