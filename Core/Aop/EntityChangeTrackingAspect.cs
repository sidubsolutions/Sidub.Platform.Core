﻿/*
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

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.ChangeTracking;

#endregion

namespace Sidub.Platform.Core.Aop
{

    /// <summary>
    /// Represents an aspect for tracking changes in entity properties.
    /// </summary>
    public partial class EntityChangeTrackingAspect : TypeAspect
    {

        /// <summary>
        /// Builds the aspect.
        /// </summary>
        /// <param name="builder">The aspect builder.</param>
        [CompileTime]
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            foreach (var property in builder.Target.Properties.Where(IsPropertyEligible))
            {
                builder.Advice.Override(property, nameof(OverrideProperty));
            }
        }

        /// <summary>
        /// Checks if a property is eligible for change tracking.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns><c>true</c> if the property is eligible for change tracking; otherwise, <c>false</c>.</returns>
        [CompileTime]
        private static bool IsPropertyEligible(IProperty property)
        {
            Func<bool> hasEligibleInterface = () => property.Attributes.Any(attr => attr.Type.AllImplementedInterfaces.Any(intr => intr.Is(typeof(IEntityField))));
            bool isEligibleType = true;

            return isEligibleType && hasEligibleInterface();
        }

        /// <summary>
        /// Overrides the property getter and setter to track changes.
        /// </summary>
        [Template]
        protected dynamic? OverrideProperty
        {
            get => meta.Proceed();
            set
            {
                IEntityChangeTracking entity = meta.This
                    ?? throw new Exception("Null encountered casting to IEntityChangeTracking.");

                if (entity.SuppressChangeTracking)
                {
                    meta.Proceed();
                    return;
                }

                var beforeChange = meta.Target.Property.Value;

                meta.Proceed();

                var afterChange = meta.Target.Property.Value;

                if (beforeChange != afterChange)
                {
                    var field = meta.RunTime(meta.Target.Property.ToFieldOrPropertyInfo()).GetCustomAttributes(true).SingleOrDefault(x => x is IEntityField) as IEntityField
                        ?? throw new Exception("Unable to retrieve field info.");

                    if (!entity.OriginalValues.ContainsKey(field))
                        entity.OriginalValues.Add(field, beforeChange);

                    meta.This.OnEntityFieldChanged(field, beforeChange, afterChange);
                }
            }
        }

    }

}


