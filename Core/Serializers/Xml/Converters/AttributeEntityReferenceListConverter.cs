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
using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml.Converters
{

    /// <summary>
    /// Converts a list of entity references to XML and vice versa.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    public class AttributeEntityReferenceListConverter<TRelated> : IAttributeEntityReferenceListConverter
            where TRelated : IEntity
    {

        #region Public methods

        /// <inheritdoc/>
        public IEntityReferenceList Read(ref XmlReader reader, XmlEntitySerializerOptions options)
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

                if (reader.NodeType == XmlNodeType.Element)
                {
                    // increment the depth...
                    if (!reader.IsEmptyElement)
                        depth++;
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // decrement the depth...
                    depth--;

                    // end object at depth 0 means we're at the ending of the TRelated object, end / return...
                    if (depth == 1)
                    {
                        break;
                    }
                }

                // for each object contained in the list...
                if (reader.NodeType == XmlNodeType.Element && depth == 2)
                {
                    var builder = AttributeEntityReferenceConverter.Create<TRelated>();
                    var referenceObject = builder.Read(ref reader, options) as EntityReference<TRelated>
                        ?? throw new Exception("Could not get correctly typed entity reference.");

                    relatedEntities.Add(referenceObject);
                }
            }

            return relatedEntities;
        }

        /// <inheritdoc/>
        public void Write(XmlWriter writer, IEntityReferenceList value, XmlEntitySerializerOptions options)
        {
            //writer.WriteStartElement(EntityTypeHelper.GetEntityName(value));

            foreach (var i in value)
            {

                // intention is we simply iterate each and call the serializer, the AttributeEntityReferenceConverter should handle...
                var builder = AttributeEntityReferenceConverter.Create<TRelated>();
                builder.Write(writer, i, options);

            }

            //writer.WriteEndElement();
        }

        #endregion

    }

    public static class AttributeEntityReferenceListConverter
    {

        #region Public static methods

        /// <summary>
        /// Creates an instance of <see cref="IAttributeEntityReferenceListConverter"/> for the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>An instance of <see cref="IAttributeEntityReferenceListConverter"/>.</returns>
        public static IAttributeEntityReferenceListConverter Create(Type entityType)
        {
            var converterType = typeof(AttributeEntityReferenceListConverter<>).MakeGenericType(entityType);
            return (IAttributeEntityReferenceListConverter)Activator.CreateInstance(converterType);
        }

        #endregion

    }

}
