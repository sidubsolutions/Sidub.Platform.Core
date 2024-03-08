#region Imports

using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;
using Sidub.Platform.Core.Entity.ChangeTracking;

#endregion

namespace Sidub.Platform.Core.Aop
{

    /// <summary>
    /// Fabric class responsible for adding the EntityChangeTrackingAspect to eligible types in the project.
    /// </summary>
    internal class EntityChangeTrackingFabric : TransitiveProjectFabric
    {

        /// <summary>
        /// Amends the project by adding the EntityChangeTrackingAspect to eligible types.
        /// </summary>
        /// <param name="amender">The project amender.</param>
        public override void AmendProject(IProjectAmender amender)
        {
            var typesReceiver = amender.Outbound.SelectMany(proj => proj.Types.Where(IsTypeEligible));

            typesReceiver.AddAspect<EntityChangeTrackingAspect>();
        }

        /// <summary>
        /// Checks if the given type is eligible for EntityChangeTrackingAspect.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is eligible; otherwise, <c>false</c>.</returns>
        private bool IsTypeEligible(INamedType type)
        {
            Func<bool> hasEligibleInterface = () => type.AllImplementedInterfaces.Any(x => x.Is(typeof(IEntityChangeTracking)));
            bool isAbstract = type.IsAbstract;

            return !isAbstract && hasEligibleInterface();
        }

    }

}
