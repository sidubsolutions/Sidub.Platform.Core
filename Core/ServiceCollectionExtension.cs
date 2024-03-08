#region Imports

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sidub.Platform.Core.Entity.Partitions;
using Sidub.Platform.Core.Serializers;
using Sidub.Platform.Core.Serializers.Json;
using Sidub.Platform.Core.Serializers.Xml;
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

            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, AttributeEntityJsonSerializer>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, AttributeEntityXmlSerializer>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEntitySerializer, EntityJsonSerializer>());
            services.TryAddTransient<IEntitySerializerService, EntitySerializerService>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<ILocalizationResource, GlobalResources>());

            return services;
        }

        #endregion

    }
}
