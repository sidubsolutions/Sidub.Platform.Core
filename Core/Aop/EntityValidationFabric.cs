#region Imports

using Metalama.Framework.Code;
using Metalama.Framework.Fabrics;
using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Aop
{

    /// <summary>
    /// Fabric class responsible for amending the project with the EntityValidationAspect aspect for types that are eligible.
    /// </summary>
    internal class EntityPublicParameterlessConstructorFabric : TransitiveProjectFabric
    {

        /// <summary>
        /// Amends the project by adding the EntityValidationAspect aspect to eligible types.
        /// </summary>
        /// <param name="amender">The project amender.</param>
        public override void AmendProject(IProjectAmender amender)
        {
            var typesReceiver = amender.Outbound.SelectMany(proj => proj.Types.Where(IsTypeEligible));

            typesReceiver.AddAspect<EntityValidationAspect>();
        }

        /// <summary>
        /// Checks if the given type is eligible for the EntityValidationAspect.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is eligible; otherwise, <c>false</c>.</returns>
        private bool IsTypeEligible(INamedType type)
        {
            Func<bool> hasEligibleInterface = () => type.Is(typeof(IEntity));
            bool isAbstract = type.IsAbstract;

            return !isAbstract && hasEligibleInterface();
        }

    }

}
