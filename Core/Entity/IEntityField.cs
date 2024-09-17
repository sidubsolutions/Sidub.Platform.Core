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

namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents an entity field.
    /// </summary>
    public interface IEntityField
    {

        #region Interface properties

        /// <summary>
        /// Gets a value indicating whether this field is a key field.
        /// </summary>
        bool IsKeyField { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Gets the ordinal position of the field.
        /// </summary>
        int OrdinalPosition { get; }

        #endregion

    }

}
