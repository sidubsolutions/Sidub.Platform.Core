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

using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute used to mark a property as an entity key field.
    /// </summary>
    /// <typeparam name="TValue">The type of the entity key field.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityKeyAttribute<TValue> : EntityFieldAttribute<TValue>, IEntityField
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityKeyAttribute{TValue}"/> class.
        /// </summary>
        /// <param name="name">The name of the entity key field.</param>
        /// <param name="label">The label of the entity key field.</param>
        public EntityKeyAttribute(string name, string? label = null, int ordinalPosition = 0) : base(name, label)
        {
            OrdinalPosition = ordinalPosition;
        }

        #endregion

        #region IEntityField implementation

        /// <summary>
        /// Gets a value indicating whether the field is a key field.
        /// </summary>
        bool IEntityField.IsKeyField => true;

        #endregion
    }

}
