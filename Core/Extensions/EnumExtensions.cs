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

using System.ComponentModel;
using System.Reflection;

#endregion

namespace Sidub.Platform.Core.Extensions
{

    /// <summary>
    /// Provides extension methods for working with enums.
    /// </summary>
    public static class EnumExtensions
    {

        #region Public static methods

        /// <summary>
        /// Retrieves the individual flags of an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="e">The enum value.</param>
        /// <returns>An enumerable of individual flags.</returns>
        public static IEnumerable<T> GetFlags<T>(this T e) where T : Enum
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).Cast<T>();
        }

        /// <summary>
        /// Retrieves the description of an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription<T>(this T enumValue)
            where T : Enum
        {
            if (!typeof(T).IsEnum)
                return string.Empty;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo is not null)
            {
                var attr = fieldInfo.GetCustomAttribute<DescriptionAttribute>(true);

                if (attr is not null)
                {
                    description = attr.Description;
                }
            }

            return description;
        }

        #endregion

    }
}
