using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class TypeDiscriminatorTest
    {

        [TestMethod]
        public void TypeDiscriminatorTest_General01()
        {
            var typ = typeof(TypeDiscriminator);
            var typDiscriminator = TypeDiscriminator.From(typ);
            var typDiscriminatorString = typDiscriminator.ToString();
            var definedType = typDiscriminator.GetDefinedType();

            Assert.AreEqual("Sidub.Platform.Core.TypeDiscriminator, Sidub.Platform.Core", typDiscriminatorString);
            Assert.IsTrue(typDiscriminator.GetAssemblyDiscriminator().IsAssemblyLoaded());
            Assert.IsNotNull(definedType);

            var right = TypeDiscriminator.FromString(typDiscriminatorString);
            Assert.AreEqual(typDiscriminator.AssemblyName, right.AssemblyName);
            Assert.AreEqual(typDiscriminator.TypeName, right.TypeName);
            Assert.AreEqual(typDiscriminator.AssemblyVersion, right.AssemblyVersion);
        }

        [TestMethod]
        public void TypeDiscriminatorTest_General02()
        {
            var typ = typeof(TypeDiscriminator);
            var typDiscriminator = TypeDiscriminator.From(typ, true);
            var typDiscriminatorString = typDiscriminator.ToString();
            var definedType = typDiscriminator.GetDefinedType();
            var version = typ.Assembly.GetName().Version?.ToString();

            Assert.AreEqual("Sidub.Platform.Core.TypeDiscriminator, Sidub.Platform.Core, " + version, typDiscriminatorString);
            Assert.IsTrue(typDiscriminator.GetAssemblyDiscriminator().IsAssemblyLoaded());
            Assert.IsNotNull(definedType);

            var right = TypeDiscriminator.FromString(typDiscriminatorString);
            Assert.AreEqual(typDiscriminator.AssemblyName, right.AssemblyName);
            Assert.AreEqual(typDiscriminator.TypeName, right.TypeName);
            Assert.AreEqual(typDiscriminator.AssemblyVersion, right.AssemblyVersion);
        }

    }

}
