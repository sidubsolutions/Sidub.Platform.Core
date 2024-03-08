#region Imports

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts a nullable <see cref="DateTime"/> to and from JSON.
    /// </summary>
    public class DateTimeNullableJsonConverter : JsonConverter<DateTime?>
    {
        #region Public methods

        /// <summary>
        /// Reads the JSON representation of the nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
        /// <param name="typeToConvert">The type of the object to convert.</param>
        /// <param name="options">The serializer options to use.</param>
        /// <returns>The deserialized nullable <see cref="DateTime"/>.</returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();

            if (string.IsNullOrEmpty(dateString))
                return null;

            var result = DateTime.Parse(dateString);

            return result;
        }

        /// <summary>
        /// Writes the JSON representation of the nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
        /// <param name="value">The nullable <see cref="DateTime"/> value to write.</param>
        /// <param name="options">The serializer options to use.</param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToUniversalTime().ToString("o"));
        }

        #endregion

    }

}
