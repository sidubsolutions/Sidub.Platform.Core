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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sidub.Platform.Core.Entity.Partitions;
using Sidub.Platform.Core.Serializers;
using Sidub.Platform.Core.Services;
using Sidub.Platform.Localization;

#endregion

namespace Sidub.Platform.Core
{

    /// <summary>
    /// Static helper class providing IServiceCollection extensions.
    /// </summary>
    public static class ServiceCollectionExtension
    {

        #region Extension methods

        /// <summary>
        /// Registers the Sidub filter system within the container. Initializes base configuration.
        /// </summary>
        /// <param name="services">IServiceCollection extension.</param>
        /// <param name="parser">Filter parser configuration type.</param>
        /// <returns>IServiceCollection result.</returns>
        public static IServiceCollection AddSidubPlatform(
            this IServiceCollection services,
            Func<IServiceProvider, IServiceRegistry>? metadataServiceBuilder = null)
        {
            // initialize metadata...
            if (metadataServiceBuilder is null)
                services.TryAddScoped<IServiceRegistry, InMemoryServiceRegistry>();
            else
                services.TryAddScoped(metadataServiceBuilder);

            services.TryAddTransient<IEntityPartitionService, EntityPartitionService>();

            services.TryAddTransient<IPartitionProvider, DefaultPartitionProvider>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, Serializers.Json.AttributeEntityJsonSerializer>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, Serializers.Xml.AttributeEntityXmlSerializer>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, Serializers.Json.EntityJsonSerializer>());
            services.TryAddTransient<IEntitySerializerService, EntitySerializerService>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<ILocalizationResource, GlobalResources>());

            return services;
        }

        #endregion

    }
}
