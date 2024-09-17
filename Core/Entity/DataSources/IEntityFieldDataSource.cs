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

using System.Linq.Expressions;

#endregion

namespace Sidub.Platform.Core.Entity.DataSources
{

    /// <summary>
    /// Represents a data source for entity fields.
    /// </summary>
    public interface IEntityFieldDataSource
    {

    }

    /// <summary>
    /// Represents a data source for entity fields with a specific list item type and value type.
    /// </summary>
    /// <typeparam name="TListItem">The type of the list item.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IEntityFieldDataSource<TListItem, TValue> : IEntityFieldDataSource where TListItem : notnull
    {

        #region Interface properties

        /// <summary>
        /// Gets the expression to retrieve the text value from the list item.
        /// </summary>
        Expression<Func<TListItem, string>> TextExpression { get; }

        /// <summary>
        /// Gets the expression to retrieve the value from the list item.
        /// </summary>
        Expression<Func<TListItem, TValue>> ValueExpression { get; }

        #endregion

        #region Interface methods

        /// <summary>
        /// Retrieves the list of items from the data source.
        /// </summary>
        /// <returns>The list of items.</returns>
        Task<IEnumerable<TListItem>> Get();

        #endregion

    }
}
