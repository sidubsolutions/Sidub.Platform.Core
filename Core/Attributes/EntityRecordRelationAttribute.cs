﻿#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute that defines a relationship between an entity record and another entity.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityRecordRelationAttribute<TRelated> : Attribute, IEntityRelation
        where TRelated : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the type of the related entity.
        /// </summary>
        public Type RelatedType { get; }

        /// <summary>
        /// Gets the name of the relationship.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the relationship.
        /// </summary>
        public EntityRelationshipType Relationship { get; }

        /// <summary>
        /// Gets the load type of the relationship.
        /// </summary>
        public EntityRelationLoadType LoadType { get; }

        /// <summary>
        /// Gets a value indicating whether the relationship is enumerable.
        /// </summary>
        bool IEntityRelation.IsEnumerableRelation => false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRecordRelationAttribute{TRelated}"/> class.
        /// </summary>
        /// <param name="name">The name of the relationship.</param>
        /// <param name="relationship">The type of the relationship.</param>
        /// <param name="loadType">The load type of the relationship.</param>
        public EntityRecordRelationAttribute(string name, EntityRelationshipType relationship, EntityRelationLoadType loadType = EntityRelationLoadType.Join)
        {
            RelatedType = typeof(TRelated);
            Name = name;
            Relationship = relationship;
            LoadType = loadType;
        }

        #endregion

    }

}
