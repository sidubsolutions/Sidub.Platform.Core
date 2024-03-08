#region Imports

using Sidub.Platform.Core.Identity;

#endregion

namespace Sidub.Platform.Core.Services
{

    /// <summary>
    /// In-memory implementation of the service registry.
    /// </summary>
    public class InMemoryServiceRegistry : IServiceRegistry
    {

        #region Public properties

        /// <summary>
        /// Stores ServiceReferences and associated metadata.
        /// </summary>
        public Dictionary<ServiceReference, IServiceMetadata> ServiceReferenceMetadata { get; }

        /// <summary>
        /// Stores relationships between ServiceReferences.
        /// </summary>
        public Dictionary<ServiceReference, List<ServiceReference>> ServiceReferenceRelations { get; }
        public IUserInfo? UserContext { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        public InMemoryServiceRegistry()
        {
            // configure system ServiceReferences...
            ServiceReferenceMetadata = new Dictionary<ServiceReference, IServiceMetadata>();
            ServiceReferenceRelations = new Dictionary<ServiceReference, List<ServiceReference>>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Registers a ServiceReference and its metadata against the service registry.
        /// </summary>
        /// <typeparam name="TServiceReferenceMetadata">Type of metadata.</typeparam>
        /// <param name="ServiceReference">ServiceReference to register.</param>
        /// <param name="metadata">Metadata associated with ServiceReference.</param>
        /// <param name="parent">Parent ServiceReference, if applicable.</param>
        public void RegisterServiceReference<TServiceReferenceMetadata>(ServiceReference ServiceReference, TServiceReferenceMetadata metadata, ServiceReference? parent = null) where TServiceReferenceMetadata : IServiceMetadata
        {
            // ensure the parent ServiceReference has been registered...
            if (parent is not null && !ServiceReferenceMetadata.ContainsKey(parent))
                throw new Exception($"Cannot register ServiceReference '{ServiceReference.Name}' as parent ServiceReference '{parent.Name}' is not registered.");

            // register the model and metadata...
            if (ServiceReferenceMetadata.ContainsKey(ServiceReference))
            {
                throw new Exception("TODO - dont think this should be OK...");
                //// ensure metadata of the given type has not been registered under this service...
                //if (ServiceReferenceMetadata[ServiceReference].Any(x => x is TServiceReferenceMetadata))
                //    throw new Exception($"ServiceReference '{ServiceReference.Name}' of type '{typeof(TServiceReferenceMetadata).Name}' cannot be registered. There is already metadata registered of the same type.");

                //ServiceReferenceMetadata[ServiceReference].Add(metadata);
            }
            else
                ServiceReferenceMetadata.Add(ServiceReference, metadata);

            if (parent is not null)
            {
                // associate the ServiceReference relationship...
                if (ServiceReferenceRelations.ContainsKey(parent))
                    ServiceReferenceRelations[parent].Add(ServiceReference);
                else
                    ServiceReferenceRelations.Add(parent, new[] { ServiceReference }.ToList());
            }
        }

        /// <summary>
        /// Retrieves a ServiceReference provided its type and name.
        /// </summary>
        /// <typeparam name="TServiceReference">Type of ServiceReference.</typeparam>
        /// <param name="name">Name of ServiceReference.</param>
        /// <returns>A ServiceReference, if found.</returns>
        public TServiceReference? GetServiceReference<TServiceReference>(string? name = null, ServiceReference? context = null) where TServiceReference : ServiceReference
        {
            if (context is not null && !ServiceReferenceRelations.ContainsKey(context))
                throw new Exception($"The provided service context type '{context.GetType().Name}' with name '{context.Name}' could not be found within the service registry; ensure the provided service context is properly typed and named.");

            // TODO - likely issue if nest level is greater than one... need a recursive join to pull in relation's relations and so forth...
            var ServiceReferencesToSearch = context is null ? ServiceReferenceMetadata.Select(x => x.Key) : ServiceReferenceRelations[context];

            var current = ServiceReferencesToSearch.OfType<TServiceReference>().Where(x => string.IsNullOrEmpty(name) || x.Name == name);

            TServiceReference? result = null;

            if (current.Count() > 1)
                throw new Exception("Multiple ServiceReferences found; narrow criteria.");

            result = current.SingleOrDefault();

            return result;
        }

        //public TServiceReference? GetServiceReference<TServiceReference>(IServiceMetadata metadata) where TServiceReference : ServiceReference
        //{
        //    var ServiceReferencesToSearch = ServiceReferenceMetadata.Select(x => x.Key);

        //    var current = ServiceReferencesToSearch.Single(x => ServiceReferenceMetadata[x] == metadata);
        //    //.SingleOrDefault(x => x is TServiceReference && x == metadata);

        //    return current as TServiceReference;
        //}

        /// <summary>
        /// Retrieves a ServiceReference's metadata.
        /// </summary>
        /// <typeparam name="TServiceReferenceMetadata">Type of metadata.</typeparam>
        /// <param name="context">The ServiceReference context to search; relationships will also be analyzed.</param>
        /// <returns>Enumerable of metadata, if found.</returns>
        public IEnumerable<TServiceReferenceMetadata> GetMetadata<TServiceReferenceMetadata>(ServiceReference? ServiceReference = null) where TServiceReferenceMetadata : class, IServiceMetadata
        {
            var result = Enumerable.Empty<TServiceReferenceMetadata>();
            var ServiceReferencesToProcess = ServiceReference is null
                ? ServiceReferenceMetadata.Keys.AsEnumerable()
                : new[] { ServiceReference }.AsEnumerable();

            foreach (var m in ServiceReferencesToProcess)
                result = Enumerable.Concat(result, GetServiceReferenceMetadata<TServiceReferenceMetadata>(m!));

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles recursion through ServiceReference relationships to find all matching metadata.
        /// </summary>
        /// <typeparam name="TServiceReferenceMetadata">Type of metadata.</typeparam>
        /// <param name="context">ServiceReference context to search.</param>
        /// <returns>Enumerable of metadata, if found.</returns>
        private IEnumerable<TServiceReferenceMetadata> GetServiceReferenceMetadata<TServiceReferenceMetadata>(ServiceReference context) where TServiceReferenceMetadata : class, IServiceMetadata
        {
            var currentServiceReference = context;

            if (!ServiceReferenceMetadata.ContainsKey(currentServiceReference))
                throw new Exception($"The provided service context type '{currentServiceReference.GetType().Name}' with name '{currentServiceReference.Name}' could not be found within the service registry; ensure the provided service context is properly typed and named.");

            var currentMetadata = ServiceReferenceMetadata[currentServiceReference];

            IEnumerable<TServiceReferenceMetadata> result = currentMetadata is TServiceReferenceMetadata
                ? new[] { (currentMetadata as TServiceReferenceMetadata)! }.AsEnumerable()
                : Enumerable.Empty<TServiceReferenceMetadata>();

            // iterate through ServiceReference relations to see if the desired type has been associated...
            if (ServiceReferenceRelations.ContainsKey(currentServiceReference))
            {
                foreach (var i in ServiceReferenceRelations[currentServiceReference])
                {
                    var subResult = GetServiceReferenceMetadata<TServiceReferenceMetadata>(i);
                    result = Enumerable.Concat(result, subResult);
                }
            }

            return result;
        }

        #endregion

    }

}
