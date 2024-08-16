#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;
using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml.Converters
{
    /// <summary>
    /// Converts an attribute-based entity reference to and from XML.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    public class AttributeEntityReferenceConverter<TRelated> : IAttributeEntityReferenceConverter
            where TRelated : IEntity
    {
        #region Public methods

        /// <inheritdoc/>
        public IEntityReference? Read(ref XmlReader reader, XmlEntitySerializerOptions options)
        {
            var relatedEntityKeys = EntityTypeHelper.GetEntityFields<TRelated>(EntityFieldType.Keys);

            // begin reading through the data until we finish parsing this relationship; we're dealing with a record relationship, so no need
            //  to deal with enumerables here...
            var relationshipBuilder = new EntityRelationshipBuilder<TRelated>();
            int depth = 0;
            var skipRead = true;

            IXmlNamespaceResolver resolver = new XmlNamespaceManager(new NameTable());

            // note we do not want to trigger a read on the first iteration since the reader is already
            //  positioned as expected...
            while (skipRead || reader.Read())
            {
                // clear the skipRead flag so further reads are triggered...
                skipRead = false;

                if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
                {
                    // increment depth...
                    depth++;
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
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
                if (depth == 1)
                {
                    // object depth of one means we're at the root element... 
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    var propertyName = reader.Name
                        ?? throw new Exception("Failed to get property name from related entity data.");

                    var relatedKey = relatedEntityKeys.SingleOrDefault(x => x.FieldName == propertyName);

                    if (relatedKey is not null)
                    {
                        // the property name matches a key field in our related entity, iterate the reader forward once
                        //  so we can grab the relation value...
                        var deserialized = XmlValueSerializer.Read(ref reader, relatedKey.FieldType, resolver);
                        relationshipBuilder.AddKeyValueToCurrentReference(relatedKey, deserialized);
                        skipRead = true;
                        depth--;
                    }
                    else if (propertyName == TypeDiscriminatorEntityField.Instance.FieldName)
                    {
                        // type discriminator exists...
                        var typeDiscriminator = XmlValueSerializer.Read(ref reader, typeof(string), resolver) as string
                            ?? throw new Exception("Failed to retrieve type discriminator value.");

                        relationshipBuilder.SetConcreteType(typeDiscriminator);
                        skipRead = true;
                        depth--;
                    }
                }
            }

            // at this point, the relationship builder should be populated with the information that was available from data...
            var entityReference = relationshipBuilder.CreateReference();

            return entityReference;
        }

        /// <inheritdoc/>
        public void Write(XmlWriter writer, IEntityReference value, XmlEntitySerializerOptions options)
        {
            // note, an alternative thought might be to pass the entity from the reference into the serialization layer, requesting
            //  only key serialization; however there is no guarantee the entity has actually been populated... that would require
            //  an asynchronous context to execute within, which we do not have here (not to mention performance impact)... as such
            //  we will serialize the keys directly from the reference...
            //writer.WriteStartElement(EntityTypeHelper.GetEntityName(value));
            var isAbstract = EntityTypeHelper.IsEntityAbstract<TRelated>();

            if (isAbstract)
            {
                // we need to write the type discriminator...
                var typeDiscriminator = value.ConcreteType;
                writer.WriteElementString(TypeDiscriminatorEntityField.Instance.FieldName, typeDiscriminator?.ToString());
            }

            foreach (var key in value.EntityKeys)
            {
                // assuming we have a key match...
                var fieldString = XmlValueSerializer.Write(key.Value);
                writer.WriteStartElement(key.Key.FieldName);
                writer.WriteValue(fieldString ?? string.Empty);
                writer.WriteEndElement();
            }

            //writer.WriteEndElement();
        }

        #endregion
    }

    /// <summary>
    /// Provides static methods to create instances of <see cref="AttributeEntityReferenceConverter{TRelated}"/>.
    /// </summary>
    public static class AttributeEntityReferenceConverter
    {
        #region Public static methods

        /// <summary>
        /// Creates an instance of <see cref="IAttributeEntityReferenceConverter"/> for the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>An instance of <see cref="IAttributeEntityReferenceConverter"/>.</returns>
        public static IAttributeEntityReferenceConverter Create(Type entityType)
        {
            var converterType = typeof(AttributeEntityReferenceConverter<>).MakeGenericType(entityType);
            return (IAttributeEntityReferenceConverter)Activator.CreateInstance(converterType);
        }

        /// <summary>
        /// Creates an instance of <see cref="IAttributeEntityReferenceConverter"/> for the specified related entity type.
        /// </summary>
        /// <typeparam name="TRelated">The type of the related entity.</typeparam>
        /// <returns>An instance of <see cref="IAttributeEntityReferenceConverter"/>.</returns>
        public static IAttributeEntityReferenceConverter Create<TRelated>()
            where TRelated : IEntity
        {
            return new AttributeEntityReferenceConverter<TRelated>();
        }

        #endregion
    }
}
