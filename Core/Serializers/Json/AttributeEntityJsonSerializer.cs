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

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Represents a JSON serializer for attribute entities.
    /// </summary>
    public class AttributeEntityJsonSerializer : EntityJsonSerializer
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeEntityJsonSerializer"/> class.
        /// </summary>
        public AttributeEntityJsonSerializer()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the serializer can handle the specified entity type and serialization language.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="serializationLanguage">The serialization language.</param>
        /// <returns><c>true</c> if the serializer can handle the specified entity type and serialization language; otherwise, <c>false</c>.</returns>
        public override bool IsHandled<TEntity>(SerializationLanguageType serializationLanguage)
        {
            if (serializationLanguage != SerializationLanguageType.Json)
                return false;

            var result = EntityTypeHelper.IsEntity<TEntity>();

            return result;
        }

        #endregion

    }

}
