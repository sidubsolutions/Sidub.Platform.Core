#region Imports

using Sidub.Platform.Core.Extensions;
using System.Collections;

#endregion

namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents a list of entity references for a specific or polymorphic entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityReferenceList<TEntity> : IList<EntityReference<TEntity>>, IEntityReferenceList
        where TEntity : IEntity
    {

        #region Member variables

        private readonly List<EntityReference<TEntity>> _entityReferences;
        private readonly List<EntityReference<TEntity>> _removedEntityReferences;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReferenceList{TEntity}"/> class.
        /// </summary>
        public EntityReferenceList()
        {
            _entityReferences = new List<EntityReference<TEntity>>();
            _removedEntityReferences = new List<EntityReference<TEntity>>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an entity reference to the list.
        /// </summary>
        /// <param name="item">The entity reference to add.</param>
        public void Add(EntityReference<TEntity> item)
        {
            _entityReferences.Add(item);
        }

        /// <summary>
        /// Adds an entity to the list by creating an entity reference for it.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity)
        {
            var reference = EntityReference.Create(entity);
            Add(reference);
        }

        /// <summary>
        /// Removes an entity reference from the list.
        /// </summary>
        /// <param name="item">The entity reference to remove.</param>
        /// <returns><c>true</c> if the entity reference was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(EntityReference<TEntity> item)
        {
            _entityReferences.Remove(item);
            item.Action = EntityRelationActionType.Delete;
            _removedEntityReferences.Add(item);

            return true;
        }

        /// <summary>
        /// Gets or sets the entity reference at the specified index.
        /// </summary>
        /// <param name="index">The index of the entity reference to get or set.</param>
        /// <returns>The entity reference at the specified index.</returns>
        public EntityReference<TEntity> this[int index]
        {
            get => _entityReferences[index];
            set => _entityReferences[index] = value;
        }

        /// <summary>
        /// Commits the changes made to the entity reference list.
        /// </summary>
        public void Commit()
        {
            _removedEntityReferences.Clear();
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Returns an enumerator that iterates through the entity references in the list.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the entity references in the list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entityReferences.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the entity references in the list.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the entity references in the list.</returns>
        IEnumerator<IEntityReference> IEnumerable<IEntityReference>.GetEnumerator()
        {
            return _entityReferences.GetEnumerator();
        }

        public IEnumerator<EntityReference<TEntity>> GetEnumerator()
        {
            return _entityReferences.GetEnumerator();
        }

        public int IndexOf(EntityReference<TEntity> item)
        {
            return _entityReferences.IndexOf(item);
        }

        public void Insert(int index, EntityReference<TEntity> item)
        {
            _entityReferences.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var item = _entityReferences[index];
            item.Action = EntityRelationActionType.Delete;
            _removedEntityReferences.Add(item);
            _entityReferences.RemoveAt(index);
        }

        public void Clear()
        {
            _removedEntityReferences.AddRange(_entityReferences.Select(x => x.With(y => y.Action = EntityRelationActionType.Delete)));
            _entityReferences.Clear();
        }

        public bool Contains(EntityReference<TEntity> item)
        {
            return _entityReferences.Contains(item);
        }

        public void CopyTo(EntityReference<TEntity>[] array, int arrayIndex)
        {
            _entityReferences.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEntityReferenceList implementation

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        Type IEntityReferenceList.EntityType => typeof(TEntity);

        /// <summary>
        /// Gets the removed entity references.
        /// </summary>
        IEnumerable<IEntityReference> IEntityReferenceList.RemovedReferences => _removedEntityReferences;

        public int Count => _entityReferences.Count;

        bool ICollection<EntityReference<TEntity>>.IsReadOnly => false;


        #endregion

    }
}
