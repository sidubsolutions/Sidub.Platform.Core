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
using Sidub.Platform.Core.Entity.Partitions;
using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// Entity partition service based on parition providers that are registered within
    /// the dependency injection container.
    /// </summary>
    public class EntityPartitionService : IEntityPartitionService
    {

        #region Readonly members

        /// <summary>
        /// Readonly storage for the available partition providers.
        /// </summary>
        private readonly List<IPartitionProvider> _partitionProviders;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        /// <param name="partitionProvider"></param>
        public EntityPartitionService(IEnumerable<IPartitionProvider> partitionProvider)
        {
            _partitionProviders = partitionProvider.ToList();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the partition key provided an entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="entity">Instance of entity</param>
        /// <returns>String representation of partition key.</returns>
        public string? GetPartitionValue(IEntity entity)
        {
            if (_partitionProviders.Count > 1)
                throw new NotImplementedException("Currently no support for multiple partition providers.");

            return _partitionProviders.Single()?.GetPartitionValue(entity);
        }

        public string? GetPartitionValue(IEntityReference entityReference)
        {
            if (_partitionProviders.Count > 1)
                throw new NotImplementedException("Currently no support for multiple partition providers.");

            return _partitionProviders.Single()?.GetPartitionValue(entityReference);
        }

        #endregion

    }
}
