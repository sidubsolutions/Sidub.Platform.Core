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

using Sidub.Platform.Core.Entity;

namespace Sidub.Platform.Core.Serializers
{

    /// <summary>
    /// Represents the options for an entity serializer.
    /// </summary>
    public interface IEntitySerializerOptions
    {

        #region Interface properties

        /// <summary>
        /// Gets the serialization language type.
        /// </summary>
        SerializationLanguageType Language { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to include type information during serialization.
        /// </summary>
        bool IncludeTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets the field serialization type for the entity.
        /// </summary>
        EntityFieldType FieldSerialization { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to serialize relationships of the entity.
        /// </summary>
        bool SerializeRelationships { get; set; }

        List<IEntityField> ExcludedFields { get; set; }

        #endregion

        #region Interface methods

        /// <summary>
        /// Creates a deep copy of the entity serializer options.
        /// </summary>
        /// <returns>A new instance of the entity serializer options.</returns>
        IEntitySerializerOptions Clone();

        #endregion

    }

}
