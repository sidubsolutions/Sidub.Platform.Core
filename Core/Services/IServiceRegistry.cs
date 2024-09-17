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

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// The service registry hosts the platform domain model, which defines platform ServiceReferences
    /// and associated information. Metadata pertaining to application and service is registered,
    /// stored, and accessed by platform components while performing operations.
    /// 
    /// For example, when retrieving data from the storage framework, a ServiceReference reference must be
    /// provided. The storage framework utilizes the service registry to retrieve storage services
    /// associated with the given ServiceReference.
    /// </summary>
    public interface IServiceRegistry
    {

        #region Interface methods

        /// <summary>
        /// Registers a ServiceReference and its metadata against the service registry instance.
        /// </summary>
        /// <typeparam name="TServiceReferenceMetadata">Type of metadata.</typeparam>
        /// <param name="ServiceReference">ServiceReference to register.</param>
        /// <param name="metadata">Metadata associated with ServiceReference.</param>
        /// <param name="parent">Parent ServiceReference, if applicable.</param>
        void RegisterServiceReference<TServiceReferenceMetadata>(ServiceReference ServiceReference, TServiceReferenceMetadata metadata, ServiceReference? parent = null) where TServiceReferenceMetadata : IServiceMetadata;

        /// <summary>
        /// Retrieves a ServiceReference provided its type and name.
        /// </summary>
        /// <typeparam name="TServiceReference">Type of ServiceReference.</typeparam>
        /// <param name="name">Name of ServiceReference.</param>
        /// <returns>A ServiceReference, if found.</returns>
        TServiceReference? GetServiceReference<TServiceReference>(string? name = null, ServiceReference? context = null) where TServiceReference : ServiceReference;

        /// <summary>
        /// Retrieves a ServiceReference's metadata.
        /// </summary>
        /// <typeparam name="TServiceReferenceMetadata">Type of metadata.</typeparam>
        /// <param name="context">The ServiceReference context to search; relationships will also be analyzed.</param>
        /// <returns>Enumerable of metadata, if found.</returns>
        IEnumerable<TServiceReferenceMetadata> GetMetadata<TServiceReferenceMetadata>(ServiceReference? context) where TServiceReferenceMetadata : IServiceMetadata;

        #endregion

    }
}
