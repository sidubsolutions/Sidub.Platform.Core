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

namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents the action type for an entity relation.
    /// </summary>
    public enum EntityRelationActionType
    {
        /// <summary>
        /// No action.
        /// </summary>
        None,

        /// <summary>
        /// Create action.
        /// </summary>
        Create,

        /// <summary>
        /// Update action.
        /// </summary>
        Update,

        /// <summary>
        /// Delete action.
        /// </summary>
        Delete
    }

    /// <summary>
    /// Represents the load type for an entity relation.
    /// </summary>
    public enum EntityRelationLoadType
    {
        /// <summary>
        /// Unknown load type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Eager load type.
        /// </summary>
        Eager,

        /// <summary>
        /// Lazy load type.
        /// </summary>
        Lazy,

        /// <summary>
        /// Join load type.
        /// </summary>
        Join
    }

    /// <summary>
    /// Represents the relationship type for an entity relation.
    /// </summary>
    public enum EntityRelationshipType
    {
        /// <summary>
        /// Unknown relationship type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Association relationship type.
        /// </summary>
        Association,

        /// <summary>
        /// Composition relationship type.
        /// </summary>
        Composition,

        /// <summary>
        /// Aggregation relationship type.
        /// </summary>
        Aggregation
    }

}
