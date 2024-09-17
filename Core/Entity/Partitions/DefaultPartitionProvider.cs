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

using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Entity.Partitions
{
    /// <summary>
    /// Provides the default implementation of the partition provider interface.
    /// </summary>
    public class DefaultPartitionProvider : IPartitionProvider
    {

        #region Public methods

        /// <summary>
        /// Gets the partition value for the given entity.
        /// </summary>
        /// <param name="entity">The entity for which to get the partition value.</param>
        /// <returns>The partition value.</returns>
        public string? GetPartitionValue(IEntity entity)
        {
            // check if the entity implements runtime partition strategy...
            var partitionStrategy = entity.GetType().GetInterface(nameof(IRuntimePartitionStrategy));

            if (partitionStrategy is not null)
            {
                var partitionPropertyValue = partitionStrategy.GetProperty(nameof(IRuntimePartitionStrategy.PartitionId))?.GetValue(entity) as string;

                return partitionPropertyValue;
            }

            partitionStrategy = entity.GetType().GetInterface(nameof(IGlobalPartitionStrategy));

            if (partitionStrategy is not null)
            {
                return GlobalPartitionStrategy.PartitionValue;
            }

            partitionStrategy = entity.GetType().GetInterface(nameof(ITypedPartitionStrategy));

            if (partitionStrategy is not null)
            {
                throw new NotImplementedException("Typed partition strategy is not implemented.");
            }

            return null;
        }

        /// <summary>
        /// Gets the partition value for the given entity reference.
        /// </summary>
        /// <param name="entityReference">The entity reference for which to get the partition value.</param>
        /// <returns>The partition value.</returns>
        public string? GetPartitionValue(IEntityReference entityReference)
        {
            // check if the entity implements runtime partition strategy...
            var partitionStrategy = entityReference.ConcreteType?.GetDefinedType().GetInterface(nameof(IRuntimePartitionStrategy));

            if (partitionStrategy is not null)
            {
                KeyValuePair<IEntityField, object>? keyMatch = entityReference.EntityKeys.SingleOrDefault(x => x.Key.FieldName == nameof(IRuntimePartitionStrategy.PartitionId));

                var partitionPropertyValue = keyMatch?.Value.ToString() ?? string.Empty;

                return partitionPropertyValue;
            }

            partitionStrategy = entityReference.ConcreteType?.GetDefinedType().GetInterface(nameof(IGlobalPartitionStrategy));

            if (partitionStrategy is not null)
            {
                return GlobalPartitionStrategy.PartitionValue;
            }

            partitionStrategy = entityReference.ConcreteType?.GetDefinedType().GetInterface(nameof(ITypedPartitionStrategy));

            if (partitionStrategy is not null)
            {
                throw new NotImplementedException("Typed partition strategy is not implemented.");
            }

            return null;
        }

        #endregion

    }
}
