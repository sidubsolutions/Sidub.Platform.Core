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
