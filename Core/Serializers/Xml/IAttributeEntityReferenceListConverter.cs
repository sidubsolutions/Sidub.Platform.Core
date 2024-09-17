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
