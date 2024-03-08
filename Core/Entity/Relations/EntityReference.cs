namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents a static class for creating entity references.
    /// </summary>
    public static class EntityReference
    {

        #region Public static methods

        /// <summary>
        /// Creates a null entity reference of the specified type.
        /// </summary>
        /// <param name="T">The type of the entity.</param>
        /// <returns>The null entity reference.</returns>
        public static IEntityReference CreateNull(Type T)
        {
            if (!typeof(IEntity).IsAssignableFrom(T))
                throw new ArgumentException("Provided type must implement IEntity.", nameof(T));

            var referenceType = typeof(EntityReference<>).MakeGenericType(T);
            var getNullReferenceProperty = referenceType.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Single(prop => prop.Name == "Null");

            var result = getNullReferenceProperty.GetValue(referenceType);
            var nullReference = result as IEntityReference
                ?? throw new Exception("Failed to initialize IEntityReference via. 'Null' method call.");

            return nullReference;
        }

        /// <summary>
        /// Creates an entity reference for the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity reference.</returns>
        public static EntityReference<TEntity> Create<TEntity>(TEntity entity) where TEntity : IEntity
        {
            return new EntityReference<TEntity>(entity);
        }

        #endregion

    }

    /// <summary>
    /// Represents an entity reference.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityReference<TEntity> : IEntityReference where TEntity : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating whether the entity is loaded.
        /// </summary>
        public bool IsLoaded { get; private set; } = false;

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public TEntity? Entity { get; private set; } = default;

        /// <summary>
        /// Gets the entity keys.
        /// </summary>
        public IDictionary<IEntityField, object> EntityKeys { get; }

        /// <summary>
        /// Gets or sets the provider for populating the entity reference.
        /// </summary>
        public Func<IDictionary<IEntityField, object>, Task<TEntity?>>? Provider { get; set; } = null;

        /// <summary>
        /// Gets the null entity reference.
        /// </summary>
        public static EntityReference<TEntity> Null => new();

        /// <summary>
        /// Gets or sets the concrete type of the entity.
        /// </summary>
        public TypeDiscriminator? ConcreteType { get; private set; }

        /// <summary>
        /// Gets or sets the action for the entity reference.
        /// </summary>
        public EntityRelationActionType Action { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReference{TEntity}"/> class.
        /// </summary>
        internal EntityReference()
        {
            EntityKeys = new Dictionary<IEntityField, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReference{TEntity}"/> class with the specified entity keys and concrete type.
        /// </summary>
        /// <param name="keys">The entity keys.</param>
        /// <param name="concreteType">The concrete type of the entity.</param>
        public EntityReference(IDictionary<IEntityField, object> keys, TypeDiscriminator? concreteType = null)
        {
            EntityKeys = keys;
            ConcreteType = concreteType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReference{TEntity}"/> class with the specified entity keys, provider, and concrete type.
        /// </summary>
        /// <param name="keys">The entity keys.</param>
        /// <param name="provider">The provider for populating the entity reference.</param>
        /// <param name="concreteType">The concrete type of the entity.</param>
        public EntityReference(IDictionary<IEntityField, object> keys, Func<IDictionary<IEntityField, object>, Task<TEntity?>> provider, TypeDiscriminator? concreteType = null)
        {
            EntityKeys = keys;
            Provider = provider;
            ConcreteType = concreteType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityReference{TEntity}"/> class with the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        internal EntityReference(TEntity entity) : this()
        {
            Set(entity);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs actions on the entity reference during commit.
        /// </summary>
        public void OnCommit()
        {
            Action = EntityRelationActionType.None;
        }

        /// <summary>
        /// Determines whether the entity reference has a value.
        /// </summary>
        /// <returns><c>true</c> if the entity reference has a value; otherwise, <c>false</c>.</returns>
        public bool HasValue()
        {
            return EntityKeys.Any();
        }

        /// <summary>
        /// Clears the entity reference.
        /// </summary>
        public void Clear()
        {
            Set(default);
        }

        /// <summary>
        /// Gets the entity asynchronously.
        /// </summary>
        /// <returns>The entity.</returns>
        public async Task<TEntity?> Get()
        {
            if (IsLoaded)
                return Entity;

            if (EntityKeys.Count == 0)
            {
                Entity = default;
            }
            else
            {
                if (Provider is null)
                    throw new Exception("Cannot populate entity reference when Provider is null.");

                var result = await Provider.Invoke(EntityKeys);
                Entity = result;
            }

            IsLoaded = true;

            return Entity;
        }

        /// <summary>
        /// Sets the entity reference to the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Set(TEntity? entity)
        {
            if (HasValue() && entity is null)
            {
                Action = EntityRelationActionType.Delete;
                ConcreteType = null;
            }
            else if (entity is not null)
            {
                if (HasValue())
                    Action = EntityRelationActionType.Update;
                else
                    Action = EntityRelationActionType.Create;

                ConcreteType = TypeDiscriminator.From(entity, false);
            }

            IsLoaded = true;
            Entity = entity;

            EntityKeys.Clear();

            if (entity is null)
                return;

            // note, keys should be present on the polymorphic interface so we don't specifically need to analyze
            //  the strong type...
            var keys = EntityTypeHelper.GetEntityFields<TEntity>(EntityFieldType.Keys);

            foreach (var key in keys)
            {
                var value = EntityTypeHelper.GetEntityFieldValue(entity, key)
                    ?? throw new Exception("Key value not found.");

                EntityKeys.Add(key, value);
            }
        }

        #endregion

        #region IEntityReference implementation

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        Type IEntityReference.EntityType => typeof(TEntity);

        /// <summary>
        /// Gets the entity asynchronously.
        /// </summary>
        /// <returns>The entity.</returns>
        async Task<IEntity?> IEntityReference.Get()
        {
            return await Get();
        }

        /// <summary>
        /// Gets or sets the provider for populating the entity reference.
        /// </summary>
        Func<IDictionary<IEntityField, object>, Task<IEntity?>>? IEntityReference.Provider
        {
            get
            {
                return (this as EntityReference<IEntity>)!.Provider;
            }
            set
            {
                static Func<IDictionary<IEntityField, object>, Task<TEntity?>>? WrapProvider(Func<IDictionary<IEntityField, object>, Task<IEntity?>>? provider) => async (keys) =>
                {
                    if (provider is null)
                        return default;

                    var result = await provider(keys);

                    if (result is TEntity entity)
                        return entity;
                    else throw new Exception("Invalid cast encountered in entity reference wrapper.");
                };

                var wrap = WrapProvider(value);
                Provider = wrap;
            }
        }

        #endregion

    }
}
