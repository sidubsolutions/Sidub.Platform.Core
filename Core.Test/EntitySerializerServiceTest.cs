using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidub.Platform.Core.Entity.Relations;
using Sidub.Platform.Core.Serializers;
using Sidub.Platform.Core.Serializers.Json;
using Sidub.Platform.Core.Services;
using Sidub.Platform.Core.Test.Models;
using Sidub.Platform.Core.Test.TestServiceReferences;

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class EntitySerializerServiceTest
    {

        #region Read-only members

        private readonly IServiceRegistry _entityMetadataService;
        private readonly IEntitySerializerService _entitySerializerService;

        #endregion

        #region Constructors

        public EntitySerializerServiceTest()
        {
            // initialize dependency injection environment...
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSidubPlatform();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _entityMetadataService = serviceProvider.GetService<IServiceRegistry>() ?? throw new Exception("IEntityMetadataService not initialized.");
            _entitySerializerService = serviceProvider.GetService<IEntitySerializerService>() ?? throw new Exception("IEntitySerializerService not initialized.");

            var ServiceReference = new StorageServiceReference("TestServiceReference");
            var metadata = new StorageMetadata();
            _entityMetadataService.RegisterServiceReference(ServiceReference, metadata);
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void AttributeSerializeEntity_NullGuidTest()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            var model = new TestModel04()
            {
                Id = Guid.NewGuid(),
                Value = null
            };


            var serializedResult = _entitySerializerService.Serialize(model, options);
            Assert.IsNotNull(serializedResult);

            var deserializedResult = _entitySerializerService.Deserialize<TestModel04>(serializedResult, options);
            Assert.IsNotNull(deserializedResult);

            Assert.IsNull(deserializedResult.Value);

        }

        [TestMethod]
        public void AttributeSerializeEntity_EnumTest()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            var model = new TestModel03()
            {
                Id = Guid.NewGuid(),
                Name = "ModelName",
                Color = TestModel03.TestModelColorType.Yellow
            };


            var serializedResult = _entitySerializerService.Serialize(model, options);
            Assert.IsNotNull(serializedResult);

            var deserializedResult = _entitySerializerService.Deserialize<TestModel03>(serializedResult, options);
            Assert.IsNotNull(deserializedResult);

            Assert.AreEqual("ModelName", deserializedResult.Name);
            Assert.AreEqual(TestModel03.TestModelColorType.Yellow, deserializedResult.Color);

        }

        [TestMethod]
        public void AttributeSerializeEntity_DateTimeTest()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            var model = new TestModel05()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow
            };


            var serializedResult = _entitySerializerService.Serialize(model, options);
            Assert.IsNotNull(serializedResult);

            var deserializedResult = _entitySerializerService.Deserialize<TestModel05>(serializedResult, options);
            Assert.IsNotNull(deserializedResult);

            Assert.AreEqual(model.Id, model.Id);
            Assert.AreEqual(model.CreatedOn, deserializedResult.CreatedOn);

        }

        [TestMethod]
        public void AttributeSerializeEntity01()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            var model = new TestModel()
            {
                Description = "test"
            };

            var serializedResult = _entitySerializerService.Serialize(model, options);
            Assert.IsNotNull(serializedResult);

            var deserializedResult = _entitySerializerService.Deserialize<TestModel>(serializedResult, options);
            Assert.IsNotNull(deserializedResult);

            Assert.AreEqual("test", deserializedResult.Description);
        }

        [TestMethod]
        public void AttributeSerializeEntity02()
        {
            JsonEntitySerializerOptions options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test"
            };

            var result = _entitySerializerService.SerializeDictionary(model, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.ContainsKey("Id"));
            Assert.IsTrue(result.ContainsKey("ModelDescription"));
            Assert.IsTrue(result.ContainsKey("Counter"));
            Assert.AreEqual(id.ToString("D"), result["Id"]);
            Assert.AreEqual("test", result["ModelDescription"]);
        }

        [TestMethod]
        public void AttributeSerializeEntity03()
        {
            JsonEntitySerializerOptions options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test",
                Counter = 12
            };

            var result = _entitySerializerService.SerializeDictionary(model, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.ContainsKey("Id"));
            Assert.IsTrue(result.ContainsKey("ModelDescription"));
            Assert.IsTrue(result.ContainsKey("Counter"));
            Assert.AreEqual(id.ToString("D"), result["Id"]);
            Assert.AreEqual("test", result["ModelDescription"]);
            Assert.AreEqual(12L, result["Counter"]);
        }

        [TestMethod]
        public void AttributeSerializeEntity04()
        {
            var options = SerializerOptions.New<JsonEntitySerializerOptions>();
            options.FieldSerialization = EntityFieldType.Keys;
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test",
                Counter = 12
            };

            var result = _entitySerializerService.SerializeDictionary(model, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey("Id"));
            Assert.AreEqual(id.ToString("D"), result["Id"]);
        }

        [TestMethod]
        public void AttributeSerializeEntity05()
        {
            var options = SerializerOptions.New<JsonEntitySerializerOptions>();
            options.FieldSerialization = EntityFieldType.Fields;
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test",
                Counter = 12
            };

            var result = _entitySerializerService.SerializeDictionary(model, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("ModelDescription"));
            Assert.IsTrue(result.ContainsKey("Counter"));
            Assert.AreEqual("test", result["ModelDescription"]);
            Assert.AreEqual(12L, result["Counter"]);
        }

        [TestMethod]
        public void AttributeSerializeEntity06()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test",
                Counter = 12
            };

            var result = _entitySerializerService.SerializeDictionary(model, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.ContainsKey("Id"));
            Assert.IsTrue(result.ContainsKey("ModelDescription"));
            Assert.IsTrue(result.ContainsKey("Counter"));
            Assert.AreEqual(id.ToString("D"), result["Id"]);
            Assert.AreEqual("test", result["ModelDescription"]);
            Assert.AreEqual(12L, result["Counter"]);
        }

        [TestMethod]
        public void AttributeSerializeAndDeserializeEntity01()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            Guid id = Guid.NewGuid();

            var model = new TestModel()
            {
                Id = id,
                Description = "test",
                Counter = 12
            };

            var serialization = _entitySerializerService.Serialize(model, options);
            var deserialized = _entitySerializerService.Deserialize<TestModel>(serialization, options);

            Assert.AreEqual(id, deserialized.Id);
            Assert.AreEqual("test", deserialized.Description);
            Assert.AreEqual(12, deserialized.Counter);
        }

        [TestMethod]
        public void AttributeSerializeAndDeserializeEntity02()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            Guid id = Guid.NewGuid();
            var websiteUri = new Uri("https://testuri.com/testpath/01");

            var model = new TestModel02()
            {
                Id = id,
                Description = "test",
                Counter = 12,
                WebsiteUri = websiteUri
            };

            var serialization = _entitySerializerService.Serialize(model, options);
            var deserialized = _entitySerializerService.Deserialize<TestModel02>(serialization, options);

            Assert.AreEqual(id, deserialized.Id);
            Assert.AreEqual("test", deserialized.Description);
            Assert.AreEqual(12, deserialized.Counter);
            Assert.AreEqual(websiteUri, deserialized.WebsiteUri);
        }

        [TestMethod]
        public void AttributeSerializeAndDeserializeEntity03()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            options.SerializeRelationships = true;

            var uri = new Uri("https://test.ca/");
            var navItem = new NavigationAction("id", "title", new NavigationActionTargetPage(uri)) { Description = "desc" };

            var serialization = _entitySerializerService.Serialize(navItem, options);
            var deserialized = _entitySerializerService.Deserialize<NavigationAction>(serialization, options);

            Assert.AreEqual("id", deserialized.Id);
            Assert.AreEqual("title", deserialized.Title);
            Assert.AreEqual("desc", deserialized.Description);

            Assert.IsNotNull(deserialized.Target);
            Assert.AreEqual(1, deserialized.Target.EntityKeys.Count);

        }

        [TestMethod]
        public void AttributeSerializeAndDeserializeEntity04()
        {
            var options = SerializerOptions.Default<JsonEntitySerializerOptions>();
            options.SerializeRelationships = true;
            var uri = new Uri("https://test.ca/");
            var navItem1 = new NavigationAction("id1", "title1", new NavigationActionTargetPage(uri)) { Id = "id1", Description = "desc1" };
            var navItem2 = new NavigationAction("id2", "title2", new NavigationActionTargetPage(uri)) { Id = "id2", Description = "desc2" };

            var navItem3 = new NavigationAction("id4", "title3252", new NavigationActionTargetPage(uri)) { Id = "id5", Description = "desc352" };
            var navItem4 = new NavigationAction("id7", "title2322", new NavigationActionTargetPage(uri)) { Id = "id7", Description = "desc533" };

            var items2 = new EntityReferenceList<INavigationItem>()
            {
                navItem3,
                navItem4
            };

            var innerGroup = new NavigationGroup()
            {
                Id = "innerGroup",
                Items = items2
            };

            var items = new EntityReferenceList<INavigationItem>()
            {
                navItem1,
                navItem2,
                innerGroup
            };

            var navGroup = new NavigationGroup()
            {
                Id = "parentGroup",
                Items = items,
                Description = "desc",
                Title = "title"
            };


            var serialization = _entitySerializerService.Serialize(navGroup, options);
            var deserialized = _entitySerializerService.Deserialize<NavigationGroup>(serialization, options);

            Assert.IsNotNull(deserialized);
            //Assert.IsInstanceOfType(deserialized.Target, typeof(NavigationActionTargetPage));
            //var target = deserialized.Target as NavigationActionTargetPage;
            //Assert.IsNotNull(target);
            //Assert.AreEqual(uri, target.TargetPage);
        }

        [TestMethod]
        public void GenericEntitySerializationTest01()
        {
            var container = new SpecializedContainer();
            var serializedBytes = _entitySerializerService.Serialize(container, SerializerOptions.Default(SerializationLanguageType.Json));
            var serialized = System.Text.Encoding.UTF8.GetString(serializedBytes);
        }

        #endregion

    }

}
