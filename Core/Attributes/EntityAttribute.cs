﻿/*
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


#endregion

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute that can be applied to classes or interfaces to mark them as entities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class EntityAttribute : Attribute
    {

        #region Public properties

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entity is versioned.
        /// </summary>
        public bool IsVersioned { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttribute"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityAttribute(string name)
        {
            Name = name;
            IsVersioned = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttribute"/> class with the specified name and versioned flag.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="isVersioned">A value indicating whether the entity is versioned.</param>
        public EntityAttribute(string name, bool isVersioned)
        {
            Name = name;
            IsVersioned = isVersioned;
        }

        #endregion

    }
}
