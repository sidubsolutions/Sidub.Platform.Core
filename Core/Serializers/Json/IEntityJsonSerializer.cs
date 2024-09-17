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
