#region Imports

using System.Text.Json;

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Represents an interface for serializing and deserializing entities to and from JSON.
    /// </summary>
    public interface IEntityJsonSerializer : IEntitySerializer
    {

        #region Interface methods

        /// <summary>
        /// Gets the JSON serializer options based on the entity serializer options.
        /// </summary>
        /// <param name="options">The entity serializer options.</param>
        /// <returns>The JSON serializer options.</returns>
        JsonSerializerOptions GetJsonSerializerOptions(IEntitySerializerOptions options);

        #endregion

    }
}
