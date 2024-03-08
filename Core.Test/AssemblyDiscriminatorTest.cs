using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sidub.Platform.Core.Test
{

    [TestClass]
    public class AssemblyDiscriminatorTest
    {

        [TestMethod]
        public void AssemblyDiscriminatorTest_General01()
        {
            var asm = typeof(AssemblyDiscriminator).Assembly;
            var asmDiscriminator = AssemblyDiscriminator.From(asm);
            var asmDiscriminatorString = asmDiscriminator.ToString();

            Assert.AreEqual("Sidub.Platform.Core", asmDiscriminatorString);
            Assert.IsTrue(asmDiscriminator.IsAssemblyLoaded());

            var right = AssemblyDiscriminator.From(asmDiscriminatorString);
            Assert.AreEqual(asmDiscriminator.AssemblyName, right.AssemblyName);
            Assert.AreEqual(asmDiscriminator.AssemblyVersion, right.AssemblyVersion);
        }

        [TestMethod]
        public void AssemblyDiscriminatorTest_General02()
        {
            var asm = typeof(AssemblyDiscriminator).Assembly;
            var asmDiscriminator = AssemblyDiscriminator.From(asm, true);
            var asmDiscriminatorString = asmDiscriminator.ToString();
            var version = asm.GetName().Version?.ToString();

            Assert.AreEqual("Sidub.Platform.Core, " + version, asmDiscriminatorString);
            Assert.IsTrue(asmDiscriminator.IsAssemblyLoaded());

            var right = AssemblyDiscriminator.From(asmDiscriminatorString);
            Assert.AreEqual(asmDiscriminator.AssemblyName, right.AssemblyName);
            Assert.AreEqual(asmDiscriminator.AssemblyVersion, right.AssemblyVersion);
        }

    }

}
