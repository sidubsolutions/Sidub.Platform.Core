#region Imports

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidub.Platform.Core.Test.Models;

#endregion

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class EntityTypeHelperTest
    {

        #region Test methods

        [TestMethod]
        public void GetEntityName01()
        {
            var result = EntityTypeHelper.GetEntityName<TestModel>();

            Assert.AreEqual("TestModel", result);
        }

        [TestMethod]
        public void GetEntityName02()
        {
            var entity = new TestModel();
            var result = EntityTypeHelper.GetEntityName(entity);

            Assert.AreEqual("TestModel", result);
        }

        [TestMethod]
        public void GetEntityKeyFields01()
        {
            var result = EntityTypeHelper.GetEntityFields<TestModel>(EntityFieldType.Keys);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.FieldName == "Id"));
        }

        [TestMethod]
        public void GetEntityKeyFields02()
        {
            var entity = new TestModel();
            var result = EntityTypeHelper.GetEntityFields(entity, EntityFieldType.Keys);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.FieldName == "Id"));
        }

        [TestMethod]
        public void GetEntityFields01()
        {
            var result = EntityTypeHelper.GetEntityFields<TestModel>(EntityFieldType.Fields);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.FieldName == "ModelDescription") && result.Any(x => x.FieldName == "Counter"));
        }

        [TestMethod]
        public void GetEntityFields02()
        {
            var entity = new TestModel();
            var result = EntityTypeHelper.GetEntityFields(entity, EntityFieldType.Fields);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.FieldName == "ModelDescription") && result.Any(x => x.FieldName == "Counter"));
        }

        [TestMethod]
        public void SetEntityField()
        {
            var entity = new TestModel();

            Assert.AreEqual(default, entity.Id);
            Assert.AreEqual(default!, entity.Description);

            var id = Guid.Parse("cf593c4c-7605-4520-bb6c-10bb4a52b86c");

            var idField = EntityTypeHelper.GetEntityField(entity, "Id");
            var descField = EntityTypeHelper.GetEntityField(entity, "ModelDescription");

            Assert.IsNotNull(idField);
            Assert.IsNotNull(descField);

            EntityTypeHelper.SetEntityFieldValue(entity, idField, id);
            EntityTypeHelper.SetEntityFieldValue(entity, descField, "desc01");

            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual("desc01", entity.Description);
        }

        [TestMethod]
        public void GetEntityField()
        {
            var entity = new TestModel();

            var idField = EntityTypeHelper.GetEntityField<TestModel>("Id");
            var descField = EntityTypeHelper.GetEntityField<TestModel>("ModelDescription");

            Assert.IsNotNull(idField);
            Assert.IsNotNull(descField);

            Assert.AreEqual(default(Guid), EntityTypeHelper.GetEntityFieldValue(entity, idField));
            Assert.AreEqual(default(string)!, EntityTypeHelper.GetEntityFieldValue(entity, descField));

            var id = Guid.Parse("cf593c4c-7605-4520-bb6c-10bb4a52b86c");

            EntityTypeHelper.SetEntityFieldValue(entity, idField, id);
            EntityTypeHelper.SetEntityFieldValue(entity, descField, "desc01");

            Assert.AreEqual(id, EntityTypeHelper.GetEntityFieldValue(entity, idField));
            Assert.AreEqual("desc01", EntityTypeHelper.GetEntityFieldValue(entity, descField));
        }

        #endregion

    }
}
