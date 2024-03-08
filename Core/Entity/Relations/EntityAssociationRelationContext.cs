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
