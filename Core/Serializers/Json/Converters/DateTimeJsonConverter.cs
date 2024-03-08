﻿#region Imports

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts DateTime objects to and from JSON format.
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        #region Public methods

        /// <summary>
        /// Reads and converts the JSON value to a DateTime object.
        /// </summary>
        /// <param name="reader">The reader to extract the JSON value from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The converted DateTime object.</returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();

            if (string.IsNullOrEmpty(dateString))
                throw new Exception("Empty or null value encountered for non-nullable date / time field.");

            var result = DateTime.Parse(dateString).ToUniversalTime();

            return result;
        }

        /// <summary>
        /// Writes the DateTime object to JSON format.
        /// </summary>
        /// <param name="writer">The writer to write the JSON value to.</param>
        /// <param name="value">The DateTime object to write.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
        }

        #endregion
    }

}
