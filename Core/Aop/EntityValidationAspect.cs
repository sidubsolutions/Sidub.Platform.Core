#region Imports

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Aop
{

    /// <summary>
    /// Represents an aspect that performs validation on entities.
    /// </summary>
    internal class EntityValidationAspect : TypeAspect
    {

        /// <summary>
        /// IsEntityConstructorValid diagnostic definition.
        /// </summary>
        private static readonly DiagnosticDefinition _isEntityConstructorValid
            = new("DUB0014", Severity.Error, $"Implementations of {nameof(IEntity)} must have a public parameterless constructor.");

        /// <summary>
        /// Builds the aspect for the specified target type.
        /// </summary>
        /// <param name="builder">The aspect builder.</param>
        public override void BuildAspect(IAspectBuilder<INamedType> builder)
        {
            var isConstructorValid = builder.Target.Constructors.Any(c => c.Parameters.Count == 0 && c.Accessibility == Accessibility.Public);

            if (!isConstructorValid)
            {
                builder.Diagnostics.Report(_isEntityConstructorValid);
            }
        }

    }

}
