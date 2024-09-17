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

using System.ComponentModel;

#endregion

namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Represents an entity that supports change tracking.
    /// </summary>
    public interface IEntityChangeTracking : IRevertibleChangeTracking, INotifyEntityFieldChanged, IEntity
    {

        #region Interface properties

        /// <summary>
        /// Gets the original values of the entity's fields before any changes were made.
        /// </summary>
        Dictionary<IEntityField, object?> OriginalValues { get; }

        /// <summary>
        /// Gets or sets a value indicating whether change tracking is suppressed for the entity.
        /// </summary>
        bool SuppressChangeTracking { get; set; }

        #endregion

    }
}
