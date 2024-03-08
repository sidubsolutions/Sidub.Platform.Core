#region Imports

using System.Reflection;

#endregion

namespace Sidub.Platform.Core
{

    /// <summary>
    /// Represents an assembly discriminator used for identifying and working with assemblies.
    /// </summary>
    public class AssemblyDiscriminator
    {

        #region Public properties

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the version of the assembly.
        /// </summary>
        public string? AssemblyVersion { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDiscriminator"/> class.
        /// </summary>
        private AssemblyDiscriminator()
        {
            AssemblyName = string.Empty;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the defined <see cref="AssemblyName"/> based on the provided <see cref="AssemblyName"/> and <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <returns>The defined <see cref="AssemblyName"/>.</returns>
        public AssemblyName GetDefinedAssemblyName()
        {
            var result = new AssemblyName(AssemblyName)
            {
                Version = AssemblyVersion is null ? null : new Version(AssemblyVersion)
            };

            return result;
        }

        /// <summary>
        /// Checks if the assembly is loaded in the current application domain.
        /// </summary>
        /// <returns><c>true</c> if the assembly is loaded; otherwise, <c>false</c>.</returns>
        public bool IsAssemblyLoaded()
        {
            var loaded = AppDomain.CurrentDomain.GetAssemblies()
                .Any(x => x.GetName().Name == AssemblyName
                    && (AssemblyVersion is null || x.GetName().Version?.ToString() == AssemblyVersion)
                    );

            return loaded;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Creates an <see cref="AssemblyDiscriminator"/> instance based on the provided <see cref="Assembly"/> and optional version enforcement.
        /// </summary>
        /// <param name="T">The <see cref="Assembly"/> to create the discriminator from.</param>
        /// <param name="enforceVersion">Indicates whether to enforce the version of the assembly.</param>
        /// <returns>An <see cref="AssemblyDiscriminator"/> instance.</returns>
        public static AssemblyDiscriminator From(Assembly T, bool enforceVersion = false)
        {
            return new AssemblyDiscriminator()
            {
                AssemblyName = T.GetName().Name ?? throw new Exception("Could not get assembly name."),
                AssemblyVersion = enforceVersion ? T.GetName().Version?.ToString() : null
            };

        }

        /// <summary>
        /// Creates an <see cref="AssemblyDiscriminator"/> instance based on the provided <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyName">The <see cref="AssemblyName"/> to create the discriminator from.</param>
        /// <returns>An <see cref="AssemblyDiscriminator"/> instance.</returns>
        public static AssemblyDiscriminator From(AssemblyName assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName.Name))
                throw new ArgumentException("Provided assembly name does not have a populated 'Name' property which is required.", nameof(assemblyName));

            return new AssemblyDiscriminator()
            {
                AssemblyName = assemblyName.Name,
                AssemblyVersion = assemblyName.Version?.ToString()
            };
        }

        /// <summary>
        /// Creates an <see cref="AssemblyDiscriminator"/> instance based on the provided discriminator string.
        /// </summary>
        /// <param name="discriminatorString">The discriminator string in the format "assembly, version".</param>
        /// <returns>An <see cref="AssemblyDiscriminator"/> instance.</returns>
        public static AssemblyDiscriminator From(string discriminatorString)
        {
            // assembly, version
            // Sidub.Panel.SystemApp, 1.0.2.4

            var segments = discriminatorString.Split(',').Select(x => x.Trim()).ToArray();

            if (segments.Length < 1)
                throw new Exception("Invalid assembly discriminator string; minimum one segments (assembly) is expected but less than one received.");

            var result = new AssemblyDiscriminator
            {
                AssemblyName = segments[0]
            };

            if (segments.Length - 2 >= 0)
            {
                result.AssemblyVersion = segments[1];
            }

            return result;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="AssemblyDiscriminator"/>.
        /// </summary>
        /// <returns>A string representation of the <see cref="AssemblyDiscriminator"/>.</returns>
        public override string ToString()
        {
            var result = AssemblyName;

            if (AssemblyVersion is not null)
                result += ", " + AssemblyVersion;

            return result;
        }

        #endregion

        #region Internal static methods

        /// <summary>
        /// Creates an <see cref="AssemblyDiscriminator"/> instance based on the provided assembly name and version.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="assemblyVersion">The version of the assembly.</param>
        /// <returns>An <see cref="AssemblyDiscriminator"/> instance.</returns>
        internal static AssemblyDiscriminator From(string assemblyName, string? assemblyVersion)
        {
            return new AssemblyDiscriminator() { AssemblyName = assemblyName, AssemblyVersion = assemblyVersion };
        }

        #endregion

    }

}
