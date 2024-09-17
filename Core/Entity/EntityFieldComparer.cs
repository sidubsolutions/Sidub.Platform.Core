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
    /// Compares two instances of IEntityField based on their FieldType and FieldName properties.
    /// </summary>
    public class EntityFieldComparer : IComparer<IEntityField>, IEqualityComparer<IEntityField>
    {

        #region Public methods

        /// <summary>
        /// Compares two instances of IEntityField and returns an indication of their relative order.
        /// </summary>
        /// <param name="x">The first IEntityField to compare.</param>
        /// <param name="y">The second IEntityField to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y.
        /// </returns>
        public int Compare(IEntityField x, IEntityField y)
        {
            // Check for null arguments
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            // Compare the FieldType property
            int typeComparison = string.Compare(x.FieldType.Name, y.FieldType.Name, StringComparison.OrdinalIgnoreCase);
            if (typeComparison != 0) return typeComparison;

            // Compare the FieldName property
            return string.Compare(x.FieldName, y.FieldName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether two instances of IEntityField are equal.
        /// </summary>
        /// <param name="x">The first IEntityField to compare.</param>
        /// <param name="y">The second IEntityField to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(IEntityField x, IEntityField y)
        {
            var nameMatch = string.Equals(x.FieldName, y.FieldName, StringComparison.Ordinal);
            var typeMatch = x.FieldType == y.FieldType;

            return nameMatch && typeMatch;
        }

        /// <summary>
        /// Returns a hash code for the specified IEntityField.
        /// </summary>
        /// <param name="obj">The IEntityField for which to get a hash code.</param>
        /// <returns>A hash code for the specified IEntityField.</returns>
        public int GetHashCode(IEntityField obj)
        {
            return HashCode.Combine(obj.FieldName, obj.FieldType);
        }

        #endregion

    }
}
