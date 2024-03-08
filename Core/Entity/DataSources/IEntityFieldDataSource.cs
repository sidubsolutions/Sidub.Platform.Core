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
