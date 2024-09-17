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

namespace Sidub.Platform.Core.Extensions
{

    /// <summary>
    /// Provides extension methods for fluent programming.
    /// </summary>
    public static class FluentExtensions
    {

        #region Public static methods

        /// <summary>
        /// Copies the key-value pairs from the source dictionary to the destination dictionary.
        /// If a key already exists in the destination dictionary, the value is updated.
        /// If a key does not exist in the destination dictionary, a new key-value pair is added.
        /// </summary>
        /// <typeparam name="T">The type of the destination dictionary.</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="destination">The destination dictionary.</param>
        public static void CopyTo<T>(this IDictionary<string, object> source, ref T destination) where T : IDictionary<string, object>
        {
            foreach (var sourceRecord in source)
            {
                if (destination.ContainsKey(sourceRecord.Key))
                    destination[sourceRecord.Key] = sourceRecord.Value;
                else
                    destination.Add(sourceRecord.Key, sourceRecord.Value);
            }
        }

        /// <summary>
        /// Executes the specified action on the object and returns the object itself.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="setter">The action to be executed on the object.</param>
        /// <returns>The object itself.</returns>
        public static T With<T>(this T obj, Action<T> setter)
        {
            setter(obj);

            return obj;
        }

        #endregion

    }

}
