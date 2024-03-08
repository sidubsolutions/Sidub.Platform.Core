using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sidub.Platform.Core.Extensions;
using Sidub.Platform.Core.Test.Models;

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class FluentExtensionsTest
    {

        [TestMethod]
        public void WithTest()
        {
            var model = new TestModel().With(x => x.Description = "test");

            Assert.AreEqual("test", model.Description);
        }

    }

}
