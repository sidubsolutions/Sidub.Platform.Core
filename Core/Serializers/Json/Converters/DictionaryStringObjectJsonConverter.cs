#region Imports

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts a dictionary with string keys and object values to JSON and vice versa.
    /// </summary>
    public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object?>>
    {

        #region Public methods

        /// <summary>
        /// Reads and converts JSON to a dictionary with string keys and object values.
        /// </summary>
        /// <param name="reader">The reader to extract JSON data from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The converted dictionary.</returns>
        public override Dictionary<string, object?> Read(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
            }

            var dictionary = new Dictionary<string, object?>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dictionary;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("JsonTokenType was not PropertyName");

                var propertyName = reader.GetString();

                if (string.IsNullOrWhiteSpace(propertyName) || propertyName is null)
                    throw new JsonException("Failed to get property name");

                reader.Read();

                dictionary.Add(propertyName, ExtractValue(ref reader, typeToConvert, options));
            }

            return dictionary;
        }

        /// <summary>
        /// Writes a dictionary with string keys and object values to JSON.
        /// </summary>
        /// <param name="writer">The writer to output JSON data to.</param>
        /// <param name="value">The dictionary to write.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var key in value.Keys)
            {
                HandleValue(writer, key, value[key]);
            }

            writer.WriteEndObject();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Extracts the value from the JSON reader based on the token type.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The extracted value.</returns>
        private object? ExtractValue(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    if (reader.TryGetDateTime(out var date))
                    {
                        return date;
                    }
                    return reader.GetString()!;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var result))
                    {
                        return result;
                    }
                    return reader.GetDecimal();
                case JsonTokenType.StartObject:
                    return Read(ref reader, null, options);
                case JsonTokenType.StartArray:
                    var list = new List<object?>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(ExtractValue(ref reader, typeToConvert, options));
                    }
                    return list;
                default:
                    throw new JsonException($"'{reader.TokenType}' is not supported");
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Handles writing the value to the JSON writer based on the object type.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="key">The property key.</param>
        /// <param name="objectValue">The object value.</param>
        private static void HandleValue(Utf8JsonWriter writer, string? key, object? objectValue)
        {
            if (key is not null)
            {
                writer.WritePropertyName(key);
            }

            switch (objectValue)
            {
                case string stringValue:
                    writer.WriteStringValue(stringValue);
                    break;
                case DateTime dateTime:
                    writer.WriteStringValue(dateTime);
                    break;
                case long longValue:
                    writer.WriteNumberValue(longValue);
                    break;
                case int intValue:
                    writer.WriteNumberValue(intValue);
                    break;
                case float floatValue:
                    writer.WriteNumberValue(floatValue);
                    break;
                case double doubleValue:
                    writer.WriteNumberValue(doubleValue);
                    break;
                case decimal decimalValue:
                    writer.WriteNumberValue(decimalValue);
                    break;
                case bool boolValue:
                    writer.WriteBooleanValue(boolValue);
                    break;
                case Dictionary<string, object> dict:
                    writer.WriteStartObject();
                    foreach (var item in dict)
                    {
                        HandleValue(writer, item.Key, item.Value);
                    }
                    writer.WriteEndObject();
                    break;
                case object[] array:
                    writer.WriteStartArray();
                    foreach (var item in array)
                    {
                        HandleValue(writer, item);
                    }
                    writer.WriteEndArray();
                    break;
                case IEnumerable<object> enumerable:
                    writer.WriteStartArray();

                    foreach (var item in enumerable)
                    {
                        HandleValue(writer, item);
                    }

                    writer.WriteEndArray();
                    break;
                default:
                    writer.WriteNullValue();
                    break;
            }
        }

        /// <summary>
        /// Handles writing the value to the JSON writer based on the object type.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The object value.</param>
        private static void HandleValue(Utf8JsonWriter writer, object value)
        {
            HandleValue(writer, null, value);
        }

        #endregion

    }

}
