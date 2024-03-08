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
