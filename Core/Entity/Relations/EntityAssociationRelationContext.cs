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
    /// Represents the context of an entity association relation.
    /// </summary>
    public class EntityAssociationRelationContext : IEntityRelationContext
    {

        #region Public properties

        /// <summary>
        /// Gets or sets the parent context of the relation context.
        /// </summary>
        public IEntityRelationContext? ParentContext { get; }

        /// <summary>
        /// Gets the relation associated with the context.
        /// </summary>
        public IEntityRelation Relation { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EntityAssociationRelationContext class.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="parentContext">The parent context.</param>
        public EntityAssociationRelationContext(IEntityRelation relation, IEntityRelationContext? parentContext = null)
        {
            Relation = relation;
            ParentContext = parentContext;
        }

        #endregion

    }
}
