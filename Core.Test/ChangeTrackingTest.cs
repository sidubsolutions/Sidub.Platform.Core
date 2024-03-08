using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidub.Platform.Core.Entity.ChangeTracking;
using Sidub.Platform.Core.Test.Models;

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class ChangeTrackingTest
    {

        [TestMethod]
        public void ChangeTrackingTest01()
        {
            var entity = new TestChangeTrackedEntity();
            IEntityChangeTracking entityChange = entity;
            Assert.IsFalse(entityChange.IsChanged);

            entity.Description = "Test";

            Assert.IsTrue(entityChange.IsChanged);
            Assert.AreEqual(1, entityChange.OriginalValues.Count);

        }

    }

}
