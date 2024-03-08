#region Imports


#endregion

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute that defines a relation to an enumerable entity.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityEnumerableRelationAttribute<TRelated> : Attribute, IEntityRelation
        where TRelated : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the type of the related entity.
        /// </summary>
        public Type RelatedType { get; }

        /// <summary>
        /// Gets the name of the relation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the relationship type.
        /// </summary>
        public EntityRelationshipType Relationship { get; }

        /// <summary>
        /// Gets the load type of the relation.
        /// </summary>
        public EntityRelationLoadType LoadType { get; }

        /// <summary>
        /// Gets a value indicating whether the relation is enumerable.
        /// </summary>
        bool IEntityRelation.IsEnumerableRelation => true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityEnumerableRelationAttribute{TRelated}"/> class.
        /// </summary>
        /// <param name="name">The name of the relation.</param>
        /// <param name="relationship">The relationship type.</param>
        /// <param name="loadType">The load type of the relation.</param>
        public EntityEnumerableRelationAttribute(string name, EntityRelationshipType relationship, EntityRelationLoadType loadType = EntityRelationLoadType.Join)
        {
            RelatedType = typeof(TRelated);
            Name = name;
            Relationship = relationship;
            LoadType = loadType;
        }

        #endregion

    }

}
