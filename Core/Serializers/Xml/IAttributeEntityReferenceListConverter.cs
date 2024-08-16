#region Imports

using Sidub.Platform.Core.Entity.Relations;
using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml
{

    /// <summary>
    /// Represents an interface for converting a list of entity references to and from XML.
    /// </summary>
    public interface IAttributeEntityReferenceListConverter
    {

        #region Interface methods

        /// <summary>
        /// Reads a list of entity references from the XML reader.
        /// </summary>
        /// <param name="reader">The XML reader.</param>
        /// <param name="options">The XML entity serializer options.</param>
        /// <returns>The entity reference list.</returns>
        IEntityReferenceList Read(ref XmlReader reader, XmlEntitySerializerOptions options);

        /// <summary>
        /// Writes a list of entity references to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="value">The entity reference list.</param>
        /// <param name="options">The XML entity serializer options.</param>
        void Write(XmlWriter writer, IEntityReferenceList value, XmlEntitySerializerOptions options);

        #endregion

    }

}
