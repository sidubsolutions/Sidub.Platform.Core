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

using System.Xml;

#endregion

namespace Sidub.Platform.Core.Serializers.Xml.Converters
{

    /// <summary>
    /// Provides methods for serializing and deserializing XML values.
    /// </summary>
    internal static class XmlValueSerializer
    {
        #region Internal static methods

        /// <summary>
        /// Writes the specified value to an XML string representation.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The XML string representation of the value.</returns>
        internal static string Write(object? value) => value switch
        {
            int intValue => intValue.ToString(),
            long longValue => longValue.ToString(),
            Guid guidValue => guidValue.ToString("B"),
            string stringValue => stringValue,
            byte[] byteArrayValue => Convert.ToBase64String(byteArrayValue),
            null => "",
            _ => throw new Exception($"Unhandled value type '{value?.GetType().Name}' in XML serializer."),
        };

        /// <summary>
        /// Reads an XML value from the specified XML reader.
        /// </summary>
        /// <param name="reader">The XML reader.</param>
        /// <param name="targetType">The type of the value to read.</param>
        /// <param name="resolver">The XML namespace resolver.</param>
        /// <returns>The deserialized value.</returns>
        internal static object? Read(ref XmlReader reader, Type targetType, IXmlNamespaceResolver resolver)
        {
            if (reader.NodeType != XmlNodeType.Element)
                throw new Exception($"Expected element node, but found '{reader.NodeType}'.");

            switch (targetType)
            {
                case Type T when T == typeof(Guid):
                    var stringResult = reader.ReadElementContentAsString();
                    var guidResult = Guid.Parse(stringResult);

                    return guidResult;

                default:
                    var objectResult = reader.ReadElementContentAs(targetType, resolver);

                    return objectResult;
            }
        }

        #endregion
    }

}
