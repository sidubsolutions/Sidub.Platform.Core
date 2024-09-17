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
    /// Represents a data source for an entity field.
    /// </summary>
    /// <typeparam name="TListItem">The type of the list item.</typeparam>
    /// <typeparam name="TValue">The type of the field value.</typeparam>
    public class EntityFieldDataSource<TListItem, TValue> : IEntityFieldDataSource<TListItem, TValue> where TListItem : notnull
    {

        #region Member variables

        private readonly IEnumerable<TListItem> _value;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the expression used to retrieve the text value of the list item.
        /// </summary>
        public Expression<Func<TListItem, string>> TextExpression { get; }

        /// <summary>
        /// Gets the expression used to retrieve the value of the list item.
        /// </summary>
        public Expression<Func<TListItem, TValue>> ValueExpression { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldDataSource{TListItem, TValue}"/> class.
        /// </summary>
        /// <param name="value">The list of items.</param>
        /// <param name="textExpression">The expression used to retrieve the text value of the list item.</param>
        /// <param name="valueExpression">The expression used to retrieve the value of the list item.</param>
        public EntityFieldDataSource(IEnumerable<TListItem> value, Expression<Func<TListItem, string>> textExpression, Expression<Func<TListItem, TValue>> valueExpression)
        {
            _value = value;
            TextExpression = textExpression;
            ValueExpression = valueExpression;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the list of items.
        /// </summary>
        /// <returns>The list of items.</returns>
        public Task<IEnumerable<TListItem>> Get()
        {
            return Task.FromResult(_value);
        }

        #endregion

    }
}
