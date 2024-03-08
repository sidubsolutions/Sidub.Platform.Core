namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents an entity relation.
    /// </summary>
    public interface IEntityRelation
    {

        /// <summary>
        /// Gets the type of the related entity.
        /// </summary>
        Type RelatedType { get; }

        /// <summary>
        /// Gets the name of the relation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the relationship.
        /// </summary>
        EntityRelationshipType Relationship { get; }

        /// <summary>
        /// Gets the type of the relation load.
        /// </summary>
        EntityRelationLoadType LoadType { get; }

        /// <summary>
        /// Gets a value indicating whether the relation is enumerable.
        /// </summary>
        bool IsEnumerableRelation { get; }

    }

}
