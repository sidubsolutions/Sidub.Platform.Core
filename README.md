# Sidub Platform - Core

This repository contains the core module for the Sidub Platform. It provides
essential concepts and functionalities that are used throughout Sidub
products.

## Main Components

### Entity framework

The entity framework provides a foundation for defining and working with
entities in the Sidub Platform. It includes interfaces and attributes for
marking entities, defining entity fields, and managing entity relationships.

### Service registry

The service registry is a central component that allows for the registration
and retrieval of services within the Sidub Platform. It provides a way to
decouple components and promote modularity and extensibility.

### Entity serialization

The entity serialization component enables the serialization and
deserialization of entities in the Sidub Platform. It provides mechanisms for
converting entities to and from various formats, such as JSON or XML.

### Entity partitioning

The entity partitioning component provides a way to partition entities into
smaller, more manageable pieces. It allows for the efficient storage and
retrieval of entities, and is particularly useful for large-scale systems.

## Usage

To use the Sidub Platform core module, you can register it within your
dependency injection container and use the provided interfaces and classes
to work with entities, services, and other core concepts.

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddSidubPlatform(metadataBuilder => 
  {
    // initialize the service metadata, etc.
  });
}
```

The `AddSidubPlatform` method registers the metadata, serialization, etc.
services within the container.

### Entity framework
Usage of the entity framework revolves around the `IEntity` interface and
related attributes. You can define entities by implementing the `IEntity`
interface and marking them with the `EntityAttribute` attribute.

The `EntityFieldAttribute` attribute is used to mark properties of an entity
as fields while the `EntityKeyAttribute` attribute is used to mark properties
as keys.

```csharp
[Entity("person")]
public class Person : IEntity
{

  [EntityKey<string>("name")]
  public string Name { get; set; }

  [EntityField<string>("age")]
  public int Age { get; set; }

}
```

Relationships between entities can be defined using the
`EntityRecordRelationAttribute` or `EntityEnumerableRelationAttribute`
attributes. Using these, you can define associations, aggregations, and
compositions between entities and control how or when they are loaded /
retrieved. Relationships must be implemented using the 
`EntityReference<TEntity>` and `EntityReferenceList<TEntity>` classes; these
provide the necessary functionality to load and manage related entities.

```csharp
[Entity("person")]
public class Person : IEntity
{

  [EntityKey<string>("name")]
  public string Name { get; set; }

  [EntityField<string>("age")]
  public int Age { get; set; }

  [EntityRecordRelationship<Address>("primaryAddress", EntityRelationType.Association)]
  public EntityReference<Address> PrimaryAddress { get; set; }

  [EntityEnumerableRelationship<Phone>("phones", EntityRelationType.Association, EntityRelationLoadType.Eager)]
  public EntityReferenceList<Phone> Phones { get; set; }

}
```

Complex data types may also be nested in lieu of a relationship; this is
achieved simply by providing a nested entity as a property of the parent 
entity.

```csharp
[Entity("person")]
public class Person : IEntity
{

  [EntityKey<string>("name")]
  public string Name { get; set; }

  [EntityField<string>("age")]
  public int Age { get; set; }

  [EntityField<Address>("primaryAddress")]
  public Address PrimaryAddress { get; set; }

}
```

#### Signed entities
Signed entities are entities that have been signed with a digital signature.
This is useful for ensuring the integrity and authenticity of entities, and
can be used to verify that an entity has not been tampered with. Signed
entities are represented with the `ISignedEntity` interface.

#### Change tracking
Change tracking is a feature that allows you to track changes to entities and
their fields. This is useful for auditing and versioning, and can be used to
keep track of changes to entities over time. Change tracking is available to
entities which implement the `IEntityChangeTracking` interface. For simplicity
of implementation, the `EntityChangeTrackingBase` base class is provided.

```csharp
[Entity("person")]
public class Person : EntityChangeTrackingBase
{

  [EntityKey<string>("name")]
  public string Name { get; set; }

  [EntityField<string>("age")]
  public int Age { get; set; }

}
```

### Service registry
The service registry is used to register and retrieve services within the
Sidub Platform. You can register services using the `RegisterService` method
and retrieve them using the `GetService` method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddSidubPlatform(metadataBuilder => 
  {
    var serviceReference = new StorageServiceReference("erp-api");
    var service = new StorageConnector("https://erp-api.example.com");

    var serviceAuthReference = new AuthenticationServiceReference("erp-auth");
    var serviceAuth = new AzureTokenCredential();

    metadataBuilder.RegisterServiceReference(serviceReference, service);
    metadataBuilder.RegisterServiceReference(serviceAuthReference, serviceAuth, serviceReference);
  });
}
```

Once services are registered, you can access them by injecting the
`IServiceRegistry` interface and accessing the 
`GetMetadata<TServiceReferenceMetadata>` method, providing the service
context. The service context is a record type object, thus any initializaiton
of a given ServiceReference with the same `Name` will match equality conditions.

```csharp
public class MyService
{

  private readonly IServiceRegistry _metadataService;

  public MyService(IServiceRegistry metadataService)
  {
    _metadataService = metadataService;
  }

  public async Task DoSomethingAsync()
  {
    var serviceReference = new StorageServiceReference("erp-api");
    var service = _metadataService.GetMetadata<StorageServiceMetadata>(serviceReference);
  }

}
```

### Entity serialization
The entity serialization component provides a way to serialize and deserialize
entities in the Sidub Platform. You can use the `SerializeEntity` and 
`DeserializeEntity` methods to convert entities to and from various formats,
such as JSON or XML.

```csharp
public class MyService
{

  private readonly IEntitySerializerService _entitySerializer;

  public MyService(IEntitySerializerService entitySerializer)
  {
    _entitySerializer = entitySerializer;
  }

  public async Task DoSomethingAsync()
  {
    var person = new Person { Name = "John", Age = 30 };
    var opts = SerializerOptions.Default(SerializationLanguageType.Json);
    var personJson = _entitySerializer.Serialize(person, opts);
    var personDeserialized = _entitySerializer.Deserialize<Person>(personJson, opts);

    var people = new List<Person> 
    { 
      new Person { Name = "John", Age = 30 },
      new Person { Name = "Jane", Age = 25 }
    };

    var peopleJson = _entitySerializer.SerializeEnumerable(people, opts);
    var peopleDeserialized = _entitySerializer.DeserializeEnumerable<Person>(peopleJson, opts);
  }

}
```

### Entity partitioning
Support for this section is minimal and documentation is being developed.

## License
This project is dual-licensed under the AGPL v3 or a proprietary license. For
details, see [https://sidub.ca/licensing](https://sidub.ca/licensing) or the 
LICENSE.txt file.
