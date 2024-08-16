#region Imports

using Sidub.Platform.Core.Entity.Relations;
using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml
{

    /// <summary>
    /// Represents an interface for converting attribute entity references to and from XML.
    /// </summary>
    public interface IAttributeEntityReferenceConverter
    {

        #region Interface methods

        /// <summary>
        /// Reads an entity reference from the XML reader.
        /// </summary>
        /// <param name="reader">The XML reader.</param>
        /// <param name="options">The XML entity serializer options.</param>
        /// <returns>The entity reference read from the XML reader.</returns>
        IEntityReference? Read(ref XmlReader reader, XmlEntitySerializerOptions options);

        /// <summary>
        /// Writes an entity reference to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="value">The entity reference to write.</param>
        /// <param name="options">The XML entity serializer options.</param>
        void Write(XmlWriter writer, IEntityReference value, XmlEntitySerializerOptions options);

        #endregion

    }

}
