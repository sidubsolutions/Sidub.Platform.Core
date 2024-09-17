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
    /// Represents a type discriminator entity field.
    /// </summary>
    /// <remarks>
    /// This field is used to differentiate between different types of entities.
    /// </remarks>
    public record TypeDiscriminatorEntityField : IEntityField
    {

        #region Member variables

        private static TypeDiscriminatorEntityField? _instance = null;
        private static readonly object _instanceLock = new object();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a value indicating whether this field is a key field.
        /// </summary>
        public bool IsKeyField => false;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string FieldName => "__sidub_entityType";

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public Type FieldType => typeof(string);

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        public string Label => "Type discriminator";

        /// <summary>
        /// Gets the ordinal position of the field.
        /// </summary>
        public int OrdinalPosition => -1;

        #endregion

        #region Static methods

        /// <summary>
        /// Gets the singleton instance of the <see cref="TypeDiscriminatorEntityField"/> class.
        /// </summary>
        public static TypeDiscriminatorEntityField Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance is null)
                        {
                            _instance = new TypeDiscriminatorEntityField();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

    }
}
