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
