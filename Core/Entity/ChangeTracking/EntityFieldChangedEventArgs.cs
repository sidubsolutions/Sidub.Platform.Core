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

namespace Sidub.Platform.Core.Entity.ChangeTracking
{

    /// <summary>
    /// Represents the event arguments for a change in an entity field.
    /// </summary>
    public class EntityFieldChangedEventArgs
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldChangedEventArgs"/> class.
        /// </summary>
        /// <param name="field">The entity field that has changed.</param>
        /// <param name="oldValue">The old value of the entity field.</param>
        /// <param name="newValue">The new value of the entity field.</param>
        public EntityFieldChangedEventArgs(IEntityField field, object? oldValue, object? newValue)
        {
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the entity field that has changed.
        /// </summary>
        public IEntityField Field { get; }

        /// <summary>
        /// Gets the old value of the entity field.
        /// </summary>
        public object? OldValue { get; }

        /// <summary>
        /// Gets the new value of the entity field.
        /// </summary>
        public object? NewValue { get; }

        #endregion

    }
}
