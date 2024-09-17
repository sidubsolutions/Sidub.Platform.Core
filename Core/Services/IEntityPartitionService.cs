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
using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Entity partition service provides data partitioning abstractions. Entities
    /// implementing a partitioning strategy will be may be categorized into a string
    /// based partition key. Partition strategies encapsulate the logic to determine
    /// a partition key based on a given entity; this separates entities from partitions
    /// and allows various partition strategies to be applied against a given entity.
    /// </summary>
    public interface IEntityPartitionService
    {

        #region Interface methods

        /// <summary>
        /// Gets the partition value for the specified entity.
        /// </summary>
        /// <param name="entity">The entity for which to get the partition value.</param>
        /// <returns>The partition value as a string.</returns>
        string? GetPartitionValue(IEntity entity);

        /// <summary>
        /// Gets the partition value for the specified entity reference.
        /// </summary>
        /// <param name="entityReference">The entity reference for which to get the partition value.</param>
        /// <returns>The partition value as a string.</returns>
        string? GetPartitionValue(IEntityReference entityReference);

        #endregion

    }
}
