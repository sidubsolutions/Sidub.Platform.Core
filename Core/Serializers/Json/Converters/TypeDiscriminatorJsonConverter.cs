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

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json.Converters
{

    /// <summary>
    /// Converts the <see cref="TypeDiscriminator"/> enum to and from JSON.
    /// </summary>
    public class TypeDiscriminatorJsonConverter : JsonConverter<TypeDiscriminator>
    {

        #region Public methods

        /// <summary>
        /// Reads the JSON representation of the <see cref="TypeDiscriminator"/> enum.
        /// </summary>
        /// <param name="reader">The reader to use.</param>
        /// <param name="typeToConvert">The type of the object to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized <see cref="TypeDiscriminator"/>.</returns>
        public override TypeDiscriminator? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeDiscriminatorString;
            typeDiscriminatorString = reader.GetString() ?? throw new Exception("Null type discriminator string.");

            var result = TypeDiscriminator.FromString(typeDiscriminatorString);

            return result;
        }

        /// <summary>
        /// Writes the JSON representation of the <see cref="TypeDiscriminator"/> enum.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, TypeDiscriminator value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

        #endregion

    }

}
