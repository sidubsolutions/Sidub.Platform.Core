#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;

#endregion

namespace Sidub.Platform.Core.Serializers
{

    /// <summary>
    /// Represents a builder for creating entity relationships.
    /// </summary>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    public class EntityRelationshipBuilder<TRelated>
        where TRelated : IEntity
    {

        #region Member variables

        private readonly List<IEntityReference> _relatedEntities;
        private EntityRelationshipBuilderRecord _currentRecord;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the relation keys of the entity.
        /// </summary>
        public Dictionary<string, IEntityField> RelationKeys { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRelationshipBuilder{TRelated}"/> class.
        /// </summary>
        public EntityRelationshipBuilder()
        {
            _relatedEntities = new List<IEntityReference>();
            _currentRecord = new EntityRelationshipBuilderRecord();

            RelationKeys = EntityTypeHelper.GetEntityFields<TRelated>(EntityFieldType.Keys).ToDictionary(relation => relation.FieldName);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a key-value pair to the current reference.
        /// </summary>
        /// <param name="field">The entity field.</param>
        /// <param name="value">The value.</param>
        public void AddKeyValueToCurrentReference(IEntityField field, object value)
        {
            _currentRecord.RelatedEntityKeyValues.Add(field, value);
        }

        /// <summary>
        /// Adds a related entity to the current reference.
        /// </summary>
        /// <param name="related">The related entity.</param>
        public void AddValueToCurrentReference(TRelated related)
        {
            _currentRecord.RelatedEntityKeyValues = EntityTypeHelper.GetEntityKeyValues(related);
            _currentRecord.RelatedEntity = related;
        }

        /// <summary>
        /// Creates a new entity reference.
        /// </summary>
        /// <returns>The entity reference.</returns>
        public EntityReference<TRelated> CreateReference()
        {
            var entityRef = new EntityReference<TRelated>(_currentRecord.RelatedEntityKeyValues, _currentRecord.ConcreteType);

            if (_currentRecord.RelatedEntity is not null)
                entityRef.Set(_currentRecord.RelatedEntity);

            return entityRef;
        }

        /// <summary>
        /// Sets the concrete type discriminator.
        /// </summary>
        /// <param name="typeDiscrimintator">The type discriminator.</param>
        public void SetConcreteType(string typeDiscrimintator)
        {
            var discriminator = TypeDiscriminator.FromString(typeDiscrimintator);

            _currentRecord.ConcreteType = discriminator;
        }

        /// <summary>
        /// Commits the current reference and adds it to the list of related entities.
        /// </summary>
        public void CommitCurrentReference()
        {
            var entityRef = new EntityReference<TRelated>(_currentRecord.RelatedEntityKeyValues);

            if (_currentRecord.RelatedEntity is not null)
                entityRef.Set(_currentRecord.RelatedEntity);

            _relatedEntities.Add(entityRef);
            _currentRecord = new EntityRelationshipBuilderRecord();
        }

        #endregion

        /// <summary>
        /// Represents a record in the entity relationship builder.
        /// </summary>
        private class EntityRelationshipBuilderRecord
        {

            #region Internal properties

            /// <summary>
            /// Gets or sets the key-value pairs of the related entity.
            /// </summary>
            internal Dictionary<IEntityField, object> RelatedEntityKeyValues { get; set; }

            /// <summary>
            /// Gets or sets the related entity.
            /// </summary>
            internal TRelated? RelatedEntity { get; set; }

            /// <summary>
            /// Gets or sets the concrete type discriminator.
            /// </summary>
            internal TypeDiscriminator? ConcreteType { get; set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityRelationshipBuilderRecord"/> class.
            /// </summary>
            internal EntityRelationshipBuilderRecord()
            {
                RelatedEntityKeyValues = new Dictionary<IEntityField, object>();
                RelatedEntity = default;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityRelationshipBuilderRecord"/> class with the specified related entity key-values and related entity.
            /// </summary>
            /// <param name="relatedEntityKeyValues">The key-value pairs of the related entity.</param>
            /// <param name="relatedEntity">The related entity.</param>
            internal EntityRelationshipBuilderRecord(Dictionary<IEntityField, object> relatedEntityKeyValues, TRelated? relatedEntity = default)
            {
                RelatedEntityKeyValues = relatedEntityKeyValues;
                RelatedEntity = relatedEntity;
            }

            #endregion

        }

    }

}
