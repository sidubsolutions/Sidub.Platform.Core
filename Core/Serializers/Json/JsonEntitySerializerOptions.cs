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

using Sidub.Platform.Core.Entity;
using System.Text.Json.Serialization;

#endregion

namespace Sidub.Platform.Core.Serializers.Json
{

    /// <summary>
    /// Represents the options for JSON entity serialization.
    /// </summary>
    public class JsonEntitySerializerOptions : IEntitySerializerOptions
    {

        #region Public properties

        /// <summary>
        /// Gets the serialization language type.
        /// </summary>
        public SerializationLanguageType Language => SerializationLanguageType.Json;

        /// <summary>
        /// Gets or sets a value indicating whether to include type information during serialization.
        /// </summary>
        public bool IncludeTypeInfo { get; set; }

        /// <summary>
        /// Gets or sets the field serialization options.
        /// </summary>
        public EntityFieldType FieldSerialization { get; set; }

        /// <summary>
        /// Gets the list of JSON converters to be used during serialization.
        /// </summary>
        public List<JsonConverter> Converters { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to serialize relationships.
        /// </summary>
        public bool SerializeRelationships { get; set; }
        public List<IEntityField> ExcludedFields { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonEntitySerializerOptions"/> class.
        /// </summary>
        public JsonEntitySerializerOptions()
        {
            IncludeTypeInfo = true;
            FieldSerialization = EntityFieldType.Keys | EntityFieldType.Fields;
            Converters = new List<JsonConverter>();
            SerializeRelationships = false;
            ExcludedFields = new List<IEntityField>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the <see cref="JsonEntitySerializerOptions"/> object.
        /// </summary>
        /// <returns>A new instance of the <see cref="JsonEntitySerializerOptions"/> class with the same property values as the original.</returns>
        public IEntitySerializerOptions Clone()
        {
            var result = new JsonEntitySerializerOptions();

            result.IncludeTypeInfo = IncludeTypeInfo;
            result.FieldSerialization = FieldSerialization;
            result.Converters.AddRange(Converters);
            result.SerializeRelationships = SerializeRelationships;
            result.ExcludedFields.AddRange(ExcludedFields);

            return result;
        }

        #endregion

    }

}
