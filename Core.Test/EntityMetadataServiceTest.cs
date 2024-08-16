#region Imports

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidub.Platform.Core.Services;
using Sidub.Platform.Core.Test.Models;
using Sidub.Platform.Core.Test.TestServiceReferences;

#endregion

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class EntityMetadataServiceTest
    {

        #region Member variables

        private readonly IServiceRegistry _entityMetadataService;
        private readonly IEntityPartitionService _entityPartitionService;

        #endregion

        #region Constructors

        public EntityMetadataServiceTest()
        {
            // initialize dependency injection environment...
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSidubPlatform();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _entityMetadataService = serviceProvider.GetService<IServiceRegistry>() ?? throw new Exception("IServiceRegistry not initialized.");
            _entityPartitionService = serviceProvider.GetService<IEntityPartitionService>() ?? throw new Exception("IEntityPartitionService not initialized.");

            var ServiceReference = new StorageServiceReference("TestStorage01");
            var metadata = new StorageMetadata();
            _entityMetadataService.RegisterServiceReference(ServiceReference, metadata);
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void RegisterAndRetrieveServiceReferenceMetadata()
        {
            var ServiceReference = new StorageServiceReference("RegisterServiceReferenceService");
            var metadata = new StorageMetadata();
            _entityMetadataService.RegisterServiceReference(ServiceReference, metadata);

            Assert.IsNotNull(_entityMetadataService.GetMetadata<StorageMetadata>(ServiceReference));
        }

        [TestMethod]
        public void RegisterAndRetrieveReferencedServiceReferenceMetadata()
        {
            var storage = new StorageServiceReference("RegisterServiceReferenceService");
            var storageMetadata = new StorageMetadata();
            _entityMetadataService.RegisterServiceReference(storage, storageMetadata);

            var auth = new AuthorizationServiceReference("AuthServiceReference");
            var authMetadata = new AuthorizationMetadata();
            _entityMetadataService.RegisterServiceReference(auth, authMetadata, storage);

            Assert.IsNotNull(_entityMetadataService.GetMetadata<StorageMetadata>(storage));
            Assert.IsNotNull(_entityMetadataService.GetMetadata<AuthorizationMetadata>(storage));
            Assert.IsNotNull(_entityMetadataService.GetMetadata<AuthorizationMetadata>(auth));
        }

        [TestMethod]
        public void RegisterDuplicateServiceReference()
        {
            var ServiceReference = _entityMetadataService.GetServiceReference<StorageServiceReference>("TestStorage01");
            Assert.IsNotNull(ServiceReference);
            Assert.IsNotNull(_entityMetadataService.GetMetadata<StorageMetadata>(ServiceReference));

            ServiceReference = new StorageServiceReference("TestStorage01");
            var metadata = new StorageMetadata();
            Exception? exception = null;

            try
            {
                _entityMetadataService.RegisterServiceReference(ServiceReference, metadata);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsNotNull(_entityMetadataService.GetMetadata<StorageMetadata>(ServiceReference));
        }

        [TestMethod]
        public void GetPartitionId01()
        {
            var entity = new RuntimePartitionedTestModel()
            {
                Id = Guid.Parse("faa6f659-6f64-4165-8e62-b51ac69efc49"),
                PartitionId = "partition",
                Description = "description"
            };

            var partition = _entityPartitionService.GetPartitionValue(entity);

            Assert.IsNotNull(partition);
            Assert.AreEqual("partition", partition);
        }

        [TestMethod]
        public void GetPartitionId02()
        {
            var entity = new GlobalPartitionedTestModel()
            {
                Id = Guid.Parse("faa6f659-6f64-4165-8e62-b51ac69efc49"),
                Description = "description"
            };

            var partition = _entityPartitionService.GetPartitionValue(entity);

            Assert.IsNotNull(partition);
            Assert.AreEqual("global", partition);
        }

        [TestMethod]
        public void GetPartitionId03()
        {
            var entity = new TestModel()
            {
                Id = Guid.Parse("faa6f659-6f64-4165-8e62-b51ac69efc49"),
                Description = "description"
            };

            var partition = _entityPartitionService.GetPartitionValue(entity);

            Assert.IsNull(partition);
        }

        #endregion

    }

}
