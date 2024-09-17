/*
 * Sidub Platform - Core
 * Copyright (C) 2024 Sidub Inc.
 * All rights reserved.
 *
 * This file is part of Sidub Platform - Core (the "Product").
 *
 * The Product is dual-licensed under:
 * 1. The GNU Affero General Public License version 3 (AGPLv3)
 * 2. Sidub Inc.'s Proprietary Software License Agreement (PSLA)
 *
 * You may choose to use, redistribute, and/or modify the Product under
 * the terms of either license.
 *
 * The Product is provided "AS IS" and "AS AVAILABLE," without any
 * warranties or conditions of any kind, either express or implied, including
 * but not limited to implied warranties or conditions of merchantability and
 * fitness for a particular purpose. See the applicable license for more
 * details.
 *
 * See the LICENSE.txt file for detailed license terms and conditions or
 * visit https://sidub.ca/licensing for a copy of the license texts.
 */

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
