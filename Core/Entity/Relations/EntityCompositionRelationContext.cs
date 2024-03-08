namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents the context of an entity composition relation.
    /// </summary>
    public class EntityCompositionRelationContext : IEntityRelationContext
    {

        #region Public properties

        /// <summary>
        /// Gets the parent context of the entity composition relation.
        /// </summary>
        public IEntityRelationContext? ParentContext { get; }

        /// <summary>
        /// Gets the entity relation associated with the context.
        /// </summary>
        public IEntityRelation Relation { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCompositionRelationContext"/> class.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="parentContext">The parent context.</param>
        public EntityCompositionRelationContext(IEntityRelation relation, IEntityRelationContext? parentContext = null)
        {
            Relation = relation;
            ParentContext = parentContext;
        }

        #endregion

    }
}
