#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.ChangeTracking;
using Sidub.Platform.Core.Entity.Partitions;
using Sidub.Platform.Core.Entity.Relations;
using System.Reflection;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core
{

    /// <summary>
    /// Provides helper functions for interacting with entity types.
    /// </summary>
    public static class EntityTypeHelper
    {

        #region Public static methods

        /// <summary>
        /// Determines whether the given object is an entity.
        /// </summary>
        /// <param name="entity">The object to check.</param>
        /// <returns>True if the object is an entity; otherwise, false.</returns>
        public static bool IsEntity(IEntity entity)
        {
            return IsEntity(entity.GetType());
        }

        /// <summary>
        /// Determines whether the entity of type TEntity is an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>True if the entity is an entity; otherwise, false.</returns>
        public static bool IsEntity<TEntity>() where TEntity : IEntity
        {
            return IsEntity(typeof(TEntity));
        }

        /// <summary>
        /// Determines whether the given entity type is an entity.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <returns>True if the given entity type is an entity; otherwise, false.</returns>
        public static bool IsEntity(Type TEntity)
        {
            return TEntity.GetInterfaces().Any(x => x == typeof(IEntity));
        }

        /// <summary>
        /// Determines whether the given entity is partitioned.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>True if the entity is partitioned; otherwise, false.</returns>
        public static bool IsEntityPartitioned(IEntity entity)
        {
            return IsEntityPartitioned(entity.GetType());
        }

        /// <summary>
        /// Determines whether the entity of type TEntity is partitioned.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>True if the entity is partitioned; otherwise, false.</returns>
        public static bool IsEntityPartitioned<TEntity>() where TEntity : IEntity
        {
            return IsEntityPartitioned(typeof(TEntity));
        }

        /// <summary>
        /// Determines whether the given entity is versioned.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>True if the entity is versioned; otherwise, false.</returns>
        public static bool IsEntityVersioned(IEntity entity)
        {
            return IsEntityVersioned(entity.GetType());
        }

        /// <summary>
        /// Determines whether the entity of type TEntity is versioned.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>True if the entity is versioned; otherwise, false.</returns>
        public static bool IsEntityVersioned<TEntity>() where TEntity : IEntity
        {
            return IsEntityVersioned(typeof(TEntity));
        }

        /// <summary>
        /// Determines whether the given entity is abstract.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>True if the entity is abstract; otherwise, false.</returns>
        public static bool IsEntityAbstract(IEntity entity)
        {
            return IsEntityAbstract(entity.GetType());
        }

        /// <summary>
        /// Determines whether the entity of type TEntity is abstract.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>True if the entity is abstract; otherwise, false.</returns>
        public static bool IsEntityAbstract<TEntity>() where TEntity : IEntity
        {
            return IsEntityAbstract(typeof(TEntity));
        }

        /// <summary>
        /// Determines whether the given entity relation is abstract.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <returns>True if the entity relation is abstract; otherwise, false.</returns>
        public static bool IsRelationshipAbstract(IEntityRelation relation)
        {
            return IsEntityAbstract(relation.RelatedType);
        }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The name of the entity.</returns>
        public static string? GetEntityName(IEntity entity)
        {
            return GetEntityName(entity.GetType());
        }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The name of the entity.</returns>
        public static string? GetEntityName<TEntity>() where TEntity : IEntity
        {
            return GetEntityName(typeof(TEntity));
        }

        /// <summary>
        /// Gets the entity field with the specified name for the given entity type.
        /// </summary>
        /// <param name="entityType">The type discriminator of the entity.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The entity field if found, otherwise null.</returns>
        public static IEntityField? GetEntityField(TypeDiscriminator entityType, string fieldName)
        {
            var definedType = entityType.GetDefinedType()
                ?? throw new Exception("No defined type was found with type discriminator.");

            if (!definedType.GetInterfaces().Any(x => x.Name == nameof(IEntity)))
                throw new Exception($"Defined type '{definedType.Name}' does not implement IEntity.");

            return GetEntityField(definedType, fieldName);
        }

        /// <summary>
        /// Gets the entity field based on the specified relation and field name.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The entity field if found; otherwise, null.</returns>
        public static IEntityField? GetEntityField(IEntityRelation relation, string fieldName)
        {
            return GetEntityField(relation.RelatedType, fieldName);
        }

        /// <summary>
        /// Gets the entity field with the specified name from the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The entity field with the specified name, or null if not found.</returns>
        public static IEntityField? GetEntityField(IEntity entity, string fieldName)
        {
            return GetEntityField(entity.GetType(), fieldName);
        }

        /// <summary>
        /// Gets the entity field with the specified name from the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The entity field with the specified name, or null if not found.</returns>
        public static IEntityField? GetEntityField<TEntity>(string fieldName)
        {
            return GetEntityField(typeof(TEntity), fieldName);
        }

        /// <summary>
        /// Gets the entity fields for the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="fieldTypes">The types of fields to retrieve.</param>
        /// <returns>The entity fields.</returns>
        public static IEnumerable<IEntityField> GetEntityFields(TypeDiscriminator entityType, EntityFieldType fieldTypes = EntityFieldType.All)
        {
            var definedType = entityType.GetDefinedType()
                ?? throw new Exception("No defined type was found with type discriminator.");

            if (!definedType.GetInterfaces().Any(x => x.Name == nameof(IEntity)))
                throw new Exception($"Defined type '{definedType.Name}' does not implement IEntity.");

            return GetEntityFields(definedType, fieldTypes);
        }

        /// <summary>
        /// Gets the entity fields for the specified entity relation.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="fieldTypes">The types of fields to retrieve.</param>
        /// <returns>The entity fields.</returns>
        public static IEnumerable<IEntityField> GetEntityFields(IEntityRelation relation, EntityFieldType fieldTypes = EntityFieldType.All)
        {
            return GetEntityFields(relation.RelatedType, fieldTypes);
        }

        /// <summary>
        /// Gets the entity fields for the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fieldTypes">The types of fields to retrieve.</param>
        /// <returns>The entity fields.</returns>
        public static IEnumerable<IEntityField> GetEntityFields(IEntity entity, EntityFieldType fieldTypes = EntityFieldType.All)
        {
            return GetEntityFields(entity.GetType(), fieldTypes);
        }

        /// <summary>
        /// Gets the entity fields for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="fieldTypes">The types of fields to retrieve.</param>
        /// <returns>The entity fields.</returns>
        public static IEnumerable<IEntityField> GetEntityFields<TEntity>(EntityFieldType fieldTypes = EntityFieldType.All) where TEntity : IEntity
        {
            return GetEntityFields(typeof(TEntity), fieldTypes);
        }

        /// <summary>
        /// Gets the value of the specified entity field from the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The entity field.</param>
        /// <returns>The value of the entity field.</returns>
        public static object? GetEntityFieldValue(IEntity entity, IEntityField field)
        {
            var fieldPropertyMap = GetFieldPropertyMap(entity.GetType());
            var property = fieldPropertyMap.Single(x => Equals(x.Key, field)).Value;

            return property?.GetValue(entity);
        }

        /// <summary>
        /// Gets the value of the specified entity field from the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The entity field.</param>
        /// <returns>The value of the entity field.</returns>
        public static TValue? GetEntityFieldValue<TValue>(IEntity entity, IEntityField field)
        {
            var value = GetEntityFieldValue(entity, field);

            if (value is TValue castValue)
                return castValue;
            else
                throw new Exception($"Entity field has type '{field.FieldType.FullName}' while the requested type is '{typeof(TValue).FullName}'.");
        }

        /// <summary>
        /// Sets the value of the specified entity field on the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The entity field.</param>
        /// <param name="value">The value to set.</param>
        public static void SetEntityFieldValue<TValue>(IEntity entity, IEntityField field, TValue? value)
        {
            var fieldPropertyMap = GetFieldPropertyMap(entity.GetType());

            var property = fieldPropertyMap.Single(x => Equals(x.Key, field)).Value;

            property.SetValue(entity, value);
        }

        /// <summary>
        /// Gets the entity relation with the specified name from the parent entity.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent entity.</typeparam>
        /// <typeparam name="TRelated">The type of the related entity.</typeparam>
        /// <param name="relationName">The name of the relation.</param>
        /// <returns>The entity relation with the specified name, or null if not found.</returns>
        public static IEntityRelation? GetEntityRelation<TParent>(string relationName)
            where TParent : class, IEntity
        {
            var relationPropertyMap = GetRelationPropertyMap(typeof(TParent));

            KeyValuePair<IEntityRelation, PropertyInfo>? record = relationPropertyMap.SingleOrDefault(x => x.Key.Name == relationName);
            var property = record?.Key;

            return property;
        }

        /// <summary>
        /// Gets the entity relations for the specified parent entity.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent entity.</typeparam>
        /// <returns>The entity relations.</returns>
        public static IEnumerable<IEntityRelation> GetEntityRelations<TParent>()
            where TParent : IEntity
        {
            var entityInterfaces = GetEntityInterfaces(typeof(TParent));

            // retrieve relation properties - properties TParent and any interfaces which have an attribute implementing IEntityRelation
            var properties = typeof(TParent)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Union(entityInterfaces.SelectMany(x => x.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)))
                .Where(x => x.GetCustomAttributes(true).Any(y => y is IEntityRelation));

            var result = properties.Select(x => x.GetCustomAttributes(true).OfType<IEntityRelation>().Single());

            return result;
        }

        /// <summary>
        /// Gets the entity relation record from the entity for the specified relation.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="relation">The entity relation.</param>
        /// <returns>The entity relation record, or null if not found.</returns>
        public static IEntityReference? GetEntityRelationRecord<TEntity>(TEntity entity, IEntityRelation relation)
            where TEntity : IEntity
        {
            var relationPropertyMap = GetRelationPropertyMap(typeof(TEntity));

            var property = relationPropertyMap.Single(x => Equals(x.Key, relation)).Value;
            var value = property.GetValue(entity);

            return value as IEntityReference;
        }

        /// <summary>
        /// Gets the entity relation enumerable from the entity for the specified relation.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="relation">The entity relation.</param>
        /// <returns>The entity relation enumerable, or null if not found.</returns>
        public static IEntityReferenceList? GetEntityRelationEnumerable<TEntity>(TEntity entity, IEntityRelation relation)
            where TEntity : IEntity
        {
            var relationPropertyMap = GetRelationPropertyMap(typeof(TEntity));

            var property = relationPropertyMap.Single(x => Equals(x.Key, relation)).Value;
            var value = property.GetValue(entity);

            return value as IEntityReferenceList;
        }

        /// <summary>
        /// Sets the entity relation reference of the specified parent entity.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent entity.</typeparam>
        /// <param name="entity">The parent entity.</param>
        /// <param name="relation">The entity relation.</param>
        /// <param name="value">The entity reference value.</param>
        /// <exception cref="Exception">Thrown when the entity relation is not implemented as an EntityReference or EntityReferenceList.</exception>
        public static void SetEntityRelationReference<TParent>(TParent entity, IEntityRelation relation, IEntityReference? value)
            where TParent : IEntity
        {
            var relationPropertyMap = GetRelationPropertyMap(typeof(TParent));

            var property = relationPropertyMap.Single(x => Equals(x.Key, relation)).Value;

            // ensure the entity field is of EntityReferenceList
            if (property.PropertyType.GetGenericTypeDefinition() != typeof(EntityReference<>))
                throw new Exception("Entity relations must be implemented as an EntityReference<TEntity> or EntityReferenceList<TEntity>, depending on relationship cardinality.");

            // if the value is null, we need to set an EntityReference null of the respective property type...
            if (value is null)
            {
                var innerT = property.PropertyType.GenericTypeArguments.Single();

                value = EntityReference.CreateNull(innerT);

            }
            else if (!property.PropertyType.IsAssignableFrom(value.GetType()))
                throw new Exception($"The provided entity reference type '{value.GetType().Name}' is not assignable to property type '{property.PropertyType.Name}'.");

            property.SetValue(entity, value);
        }

        /// <summary>
        /// Sets the entity relation reference of the specified parent entity.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent entity.</typeparam>
        /// <param name="entity">The parent entity.</param>
        /// <param name="relation">The entity relation.</param>
        /// <param name="values">The entity reference list.</param>
        /// <exception cref="Exception">Thrown when the entity relation is not implemented as an EntityReferenceList.</exception>
        public static void SetEntityRelationReference<TParent>(TParent entity, IEntityRelation relation, IEntityReferenceList values)
            where TParent : IEntity
        {
            var relationPropertyMap = GetRelationPropertyMap(typeof(TParent));

            var property = relationPropertyMap.Single(x => Equals(x.Key, relation)).Value;

            // ensure the entity field is of EntityReferenceList
            if (property.PropertyType.GetGenericTypeDefinition() != typeof(EntityReferenceList<>))
                throw new Exception("Entity relations must be implemented as an EntityReferenceList<TEntity>, depending on relationship cardinality.");

            if (!property.PropertyType.IsAssignableFrom(values.GetType()))
                throw new Exception($"The provided entity reference type '{values.GetType().Name}' is not assignable to property type '{property.PropertyType.Name}'.");

            property.SetValue(entity, values);
        }

        /// <summary>
        /// Gets the JSON converter associated with the specified entity field in the given entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="field">The entity field.</param>
        /// <returns>The JSON converter associated with the entity field, or null if not found.</returns>
        public static JsonConverter? GetEntityFieldConverter<TEntity>(IEntityField field) where TEntity : IEntity
        {
            return GetEntityFieldConverter(typeof(TEntity), field);
        }

        /// <summary>
        /// Gets the JSON converter associated with the specified entity field in the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The entity field.</param>
        /// <returns>The JSON converter associated with the entity field, or null if not found.</returns>
        public static JsonConverter? GetEntityFieldConverter(IEntity entity, IEntityField field)
        {
            return GetEntityFieldConverter(entity.GetType(), field);
        }

        /// <summary>
        /// Retrieves the key-value pairs of the specified entity.
        /// </summary>
        /// <param name="entity">The entity to retrieve the key-value pairs from.</param>
        /// <returns>A dictionary containing the entity's key-value pairs.</returns>
        public static Dictionary<IEntityField, object> GetEntityKeyValues(IEntity entity)
        {
            var fieldPropertyMap = GetFieldPropertyMap(entity.GetType()).Where(x => x.Key.IsKeyField);
            var result = new Dictionary<IEntityField, object>();

            foreach (var i in fieldPropertyMap)
            {
                result.Add(i.Key, i.Value.GetValue(entity) ?? throw new Exception("Null key encountered..?"));
            }

            return result;
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Determines whether the given entity is abstract.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <returns>True if the entity is abstract; otherwise, false.</returns>
        private static bool IsEntityAbstract(Type TEntity)
        {
            bool entityExists = false;

            entityExists = TEntity.GetInterface(nameof(IEntity)) is not null;

            var allInterfaces = TEntity.GetInterfaces();
            var directInterfaces = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));

            var entityExistsDirect = directInterfaces.Any(x => x.Name == nameof(IEntity) || x.Name == nameof(IEntitySigned) || x.Name == nameof(IEntityChangeTracking));

            if ((TEntity.IsInterface || TEntity.IsAbstract || !entityExistsDirect) && entityExists)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the specified type is an entity partitioned.
        /// </summary>
        /// <param name="TEntity">The type to check.</param>
        /// <returns><c>true</c> if the specified type is an entity partitioned; otherwise, <c>false</c>.</returns>
        private static bool IsEntityPartitioned(Type TEntity)
        {
            return TEntity.GetInterfaces().Any(x => typeof(IPartitionStrategy).IsAssignableFrom(x));
        }

        /// <summary>
        /// Determines whether the specified type is an entity versioned.
        /// </summary>
        /// <param name="TEntity">The type to check.</param>
        /// <returns><c>true</c> if the specified type is an entity versioned; otherwise, <c>false</c>.</returns>
        private static bool IsEntityVersioned(Type TEntity)
        {
            var entityInterfaces = GetEntityInterfaces(TEntity);

            var entityAttribute = TEntity.GetCustomAttribute<EntityAttribute>(true)
                                    ?? entityInterfaces.Single(x => x.GetCustomAttribute<EntityAttribute>(true) is not null).GetCustomAttribute<EntityAttribute>(true);

            if (entityAttribute is null)
                throw new Exception($"Cannot find entity attribute on type '{TEntity.Name}'.");

            return entityAttribute.IsVersioned;
        }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <returns>The name of the entity.</returns>
        private static string? GetEntityName(Type TEntity)
        {
            var entityInterfaces = GetEntityInterfaces(TEntity);

            var entityAttribute = TEntity.GetCustomAttribute<EntityAttribute>(true)
                                    ?? entityInterfaces.Single(x => x.GetCustomAttribute<EntityAttribute>(true) is not null).GetCustomAttribute<EntityAttribute>(true);

            return entityAttribute?.Name;
        }

        /// <summary>
        /// Gets the entity field with the specified name from the given entity.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The entity field with the specified name, or null if not found.</returns>
        private static IEntityField? GetEntityField(Type TEntity, string fieldName)
        {
            var fieldPropertyMap = GetFieldPropertyMap(TEntity);

            // note, oddity with "var" implying a non-nullable type w/ SingleOrDefault... explicit variable type used...
            KeyValuePair<IEntityField, PropertyInfo>? record = fieldPropertyMap.SingleOrDefault(x => x.Key.FieldName == fieldName);

            return record?.Key;
        }

        /// <summary>
        /// Retrieves the entity fields of the specified entity type based on the provided field types.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <param name="fieldTypes">The types of fields to retrieve.</param>
        /// <returns>An enumerable collection of <see cref="IEntityField"/>.</returns>
        private static IEnumerable<IEntityField> GetEntityFields(Type TEntity, EntityFieldType fieldTypes)
        {
            var fieldPropertyMap = GetFieldPropertyMap(TEntity);

            var result = fieldPropertyMap.Select(x => x.Key);

            if (fieldTypes.HasFlag(EntityFieldType.Keys) && fieldTypes.HasFlag(EntityFieldType.Fields))
                result = result.AsEnumerable();
            else if (fieldTypes.HasFlag(EntityFieldType.Keys))
                result = result.Where(x => x.IsKeyField);
            else if (fieldTypes.HasFlag(EntityFieldType.Fields))
                result = result.Where(x => !x.IsKeyField);

            return result;
        }

        /// <summary>
        /// Gets the mapping between entity fields and their corresponding property information.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <returns>The mapping between entity fields and their corresponding property information.</returns>
        private static Dictionary<IEntityField, PropertyInfo> GetFieldPropertyMap(Type TEntity)
        {
            var entityType = TEntity;
            var entityInterfaces = GetEntityInterfaces(entityType);

            var baseProperties = entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttributes(true).Any(x => x is IEntityField));

            var interfaceProperties = entityInterfaces.SelectMany(x =>
                    x.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.GetCustomAttributes(true).Any(x => x is IEntityField))
                );

            var fieldPropertyMap = baseProperties.Concat(interfaceProperties)
                .ToDictionary(x => (x.GetCustomAttributes(true).Single(x => x is IEntityField) as IEntityField)!, y => y);

            return fieldPropertyMap;
        }

        /// <summary>
        /// Gets the mapping between entity relations and their corresponding property information.
        /// </summary>
        /// <param name="T">The type of the entity.</param>
        /// <returns>The mapping between entity relations and their corresponding property information.</returns>
        private static Dictionary<IEntityRelation, PropertyInfo> GetRelationPropertyMap(Type T)
        {
            var entityType = T;
            var entityInterfaces = GetEntityInterfaces(entityType);

            var baseProperties = entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes(true).Any(x => x is IEntityRelation relation));

            var interfaceProperties = entityInterfaces.SelectMany(x =>
                    x.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.GetCustomAttributes(true).Any(x => x is IEntityRelation relation))
                );

            var relationPropertyMap = baseProperties.Concat(interfaceProperties)
                .ToDictionary(x => x.GetCustomAttributes(true).OfType<IEntityRelation>().Single(), y => y);

            return relationPropertyMap;
        }

        /// <summary>
        /// Gets the JSON converter associated with the specified entity field in the given entity type.
        /// </summary>
        /// <param name="TEntity">The type of the entity.</param>
        /// <param name="field">The entity field.</param>
        /// <returns>The JSON converter associated with the entity field, or null if not found.</returns>
        private static JsonConverter? GetEntityFieldConverter(Type TEntity, IEntityField field)
        {
            var fieldPropertyMap = GetFieldPropertyMap(TEntity);
            var property = fieldPropertyMap.SingleOrDefault(x => Equals(x.Key, field)).Value;
            var converterAttr = property?.GetCustomAttribute<JsonConverterAttribute>(true);

            if (converterAttr is not null && converterAttr.ConverterType is not null)
            {
                var converterObject = Activator.CreateInstance(converterAttr.ConverterType);

                if (converterObject is JsonConverter converter)
                    return converter;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the interfaces implemented by the specified entity type that inherit from the IEntity interface.
        /// </summary>
        /// <param name="TEntity">The entity type.</param>
        /// <returns>An enumerable collection of interface types.</returns>
        private static IEnumerable<Type> GetEntityInterfaces(Type TEntity)
        {
            IEnumerable<Type> result;

            if (TEntity.GetInterface(nameof(IEntity)) is null)
                return Enumerable.Empty<Type>();

            result = TEntity.GetInterfaces().Where(x => x.GetInterface(nameof(IEntity)) is not null);

            return result;
        }

        #endregion

    }

}
