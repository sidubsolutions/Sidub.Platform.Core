namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents the context of an entity relation.
    /// </summary>
    public interface IEntityRelationContext
    {

        /// <summary>
        /// Gets the parent context of the entity relation context.
        /// </summary>
        public IEntityRelationContext? ParentContext { get; }

        /// <summary>
        /// Gets the entity relation associated with the context.
        /// </summary>
        public IEntityRelation Relation { get; }

    }

}
