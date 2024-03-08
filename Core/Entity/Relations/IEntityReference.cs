namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents an entity reference.
    /// </summary>
    public interface IEntityReference
    {

        /// <summary>
        /// Gets a value indicating whether the entity reference is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Gets or sets the action type for the entity relation.
        /// </summary>
        EntityRelationActionType Action { get; set; }

        Type EntityType { get; }

        /// <summary>
        /// Gets the concrete type discriminator.
        /// </summary>
        public TypeDiscriminator? ConcreteType { get; }

        /// <summary>
        /// Gets the entity keys.
        /// </summary>
        public IDictionary<IEntityField, object> EntityKeys { get; }

        /// <summary>
        /// Performs actions when the entity reference is committed.
        /// </summary>
        void OnCommit();

        /// <summary>
        /// Checks if the entity reference has a value.
        /// </summary>
        /// <returns>True if the entity reference has a value, otherwise false.</returns>
        bool HasValue();

        /// <summary>
        /// Represents a provider function that retrieves the entity asynchronously.
        /// </summary>
        /// <remarks>
        /// The provider function takes a dictionary of entity keys as input and returns a task that resolves to an IEntity object.
        /// </remarks>
        Func<IDictionary<IEntityField, object>, Task<IEntity?>>? Provider { get; set; }

        /// <summary>
        /// Gets the entity asynchronously.
        /// </summary>
        /// <returns>The entity.</returns>
        public Task<IEntity?> Get();
    }

}
