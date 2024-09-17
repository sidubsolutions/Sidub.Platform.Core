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

using Sidub.Platform.Core.Serializers.Json;
using Sidub.Platform.Core.Serializers.Xml;
using System.Collections.Concurrent;

#endregion

namespace Sidub.Platform.Core.Serializers
{

    /// <summary>
    /// Provides options for entity serialization.
    /// </summary>
    public static class SerializerOptions
    {

        #region Static member variables

        private static ConcurrentBag<IEntitySerializerOptions>? _defaults;
        private static readonly object _defaultsLock = new object();

        #endregion

        #region Private static properties

        /// <summary>
        /// Gets the collection of default entity serializer options.
        /// </summary>
        private static ConcurrentBag<IEntitySerializerOptions> Defaults
        {
            get
            {
                if (_defaults is null)
                {
                    lock (_defaultsLock)
                    {
                        if (_defaults is null)
                            _defaults ??= new ConcurrentBag<IEntitySerializerOptions>();
                    }
                }

                return _defaults;
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Creates a new instance of entity serializer options based on the specified serialization language.
        /// </summary>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns>A new instance of entity serializer options.</returns>
        public static IEntitySerializerOptions New(SerializationLanguageType serializationLanguage) => serializationLanguage switch
        {
            SerializationLanguageType.Json => New<JsonEntitySerializerOptions>(),
            SerializationLanguageType.Xml => New<XmlEntitySerializerOptions>(),
            _ => throw new ArgumentException("Unhandled serialization language '' encountered.", nameof(serializationLanguage))
        };

        /// <summary>
        /// Creates a new instance of the specified entity serializer options.
        /// </summary>
        /// <typeparam name="TSerializerOptions">The type of entity serializer options.</typeparam>
        /// <returns>A new instance of the specified entity serializer options.</returns>
        public static TSerializerOptions New<TSerializerOptions>() where TSerializerOptions : class, IEntitySerializerOptions, new()
        {
            TSerializerOptions? current;

            lock (_defaultsLock)
            {
                current = Defaults.OfType<TSerializerOptions>().FirstOrDefault();

                if (current is null)
                {
                    current = new TSerializerOptions();

                    Defaults.Add(current);
                }
            }

            return (current.Clone() as TSerializerOptions)!;
        }

        /// <summary>
        /// Gets the default instance of entity serializer options based on the specified serialization language.
        /// </summary>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns>The default instance of entity serializer options.</returns>
        public static IEntitySerializerOptions Default(SerializationLanguageType serializationLanguage) => serializationLanguage switch
        {
            SerializationLanguageType.Json => Default<JsonEntitySerializerOptions>(),
            SerializationLanguageType.Xml => Default<XmlEntitySerializerOptions>(),
            _ => throw new ArgumentException($"Unhandled serialization language '{serializationLanguage}' encountered.", nameof(serializationLanguage))
        };

        /// <summary>
        /// Gets the default instance of the specified entity serializer options.
        /// </summary>
        /// <typeparam name="TSerializerOptions">The type of entity serializer options.</typeparam>
        /// <returns>The default instance of the specified entity serializer options.</returns>
        public static TSerializerOptions Default<TSerializerOptions>() where TSerializerOptions : IEntitySerializerOptions, new()
        {
            TSerializerOptions? current;

            lock (_defaultsLock)
            {
                current = Defaults.OfType<TSerializerOptions>().FirstOrDefault();

                if (current is null)
                {
                    current = new TSerializerOptions();

                    Defaults.Add(current);
                }
            }

            return current;
        }

        #endregion

    }
}
