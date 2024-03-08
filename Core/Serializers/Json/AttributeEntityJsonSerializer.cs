#region Imports

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Represents a JSON serializer for attribute entities.
    /// </summary>
    public class AttributeEntityJsonSerializer : EntityJsonSerializer
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityJsonSerializer"/> class.
        /// </summary>
        public AttributeEntityJsonSerializer()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the serializer can handle the specified entity type and serialization language.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns><c>true</c> if the serializer can handle the specified entity type and serialization language; otherwise, <c>false</c>.</returns>
        public override bool IsHandled<TEntity>(SerializationLanguageType serializationLanguage)
        {
            if (serializationLanguage != SerializationLanguageType.Json)
                return false;

            var result = EntityTypeHelper.IsEntity<TEntity>();

            return result;
        }

        #endregion

    }

}
