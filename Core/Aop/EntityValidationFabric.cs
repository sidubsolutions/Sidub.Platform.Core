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

#region Imports

using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;
using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Aop
{

    /// <summary>
    /// Fabric class responsible for amending the project with the EntityValidationAspect aspect for types that are eligible.
    /// </summary>
    internal class EntityPublicParameterlessConstructorFabric : TransitiveProjectFabric
    {

        /// <summary>
        /// Amends the project by adding the EntityValidationAspect aspect to eligible types.
        /// </summary>
        /// <param name="amender">The project amender.</param>
        public override void AmendProject(IProjectAmender amender)
        {
            var typesReceiver = amender.SelectMany(proj => proj.Types.Where(IsTypeEligible));

            typesReceiver.AddAspect<EntityValidationAspect>();
        }

        /// <summary>
        /// Checks if the given type is eligible for the EntityValidationAspect.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is eligible; otherwise, <c>false</c>.</returns>
        private bool IsTypeEligible(INamedType type)
        {
            bool hasEligibleInterface() => type.Is(typeof(IEntity));
            bool isAbstract = type.IsAbstract;

            return !isAbstract && hasEligibleInterface();
        }

    }

}
