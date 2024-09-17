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

using System.Reflection;

#endregion

namespace Sidub.Platform.Core
{

    /// <summary>
    /// Represents a type discriminator used for identifying and working with types.
    /// </summary>
    public class TypeDiscriminator
    {

        #region Public properties

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the version of the assembly.
        /// </summary>
        public string? AssemblyVersion { get; set; }

        /// <summary>
        /// Gets or sets the generic types associated with the type discriminator.
        /// </summary>
        public TypeDiscriminator[] GenericTypes { get; set; }

        /// <summary>
        /// Gets a value indicating whether the type discriminator represents a generic type.
        /// </summary>
        public bool IsGeneric { get => GenericTypes.Any(); }

        #endregion

        #region Constructors

        private TypeDiscriminator()
        {
            AssemblyName = string.Empty;
            TypeName = string.Empty;
            GenericTypes = Array.Empty<TypeDiscriminator>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the assembly discriminator associated with the type discriminator.
        /// </summary>
        /// <returns>The assembly discriminator.</returns>
        public AssemblyDiscriminator GetAssemblyDiscriminator()
        {
            return AssemblyDiscriminator.From(AssemblyName, AssemblyVersion);
        }

        /// <summary>
        /// Gets the defined type represented by the type discriminator.
        /// </summary>
        /// <returns>The defined type.</returns>
        public Type GetDefinedType()
        {
            var type = Type.GetType(TypeName + ", " + AssemblyName);

            if (type is null)
            {
                IEnumerable<Assembly> assemblies;

                var allAsms = AppDomain.CurrentDomain.GetAssemblies();

                assemblies = allAsms
                    .Where(x =>
                        x.GetName().Name == AssemblyName &&
                        (AssemblyVersion is null || x.GetName().Version?.ToString() == AssemblyVersion)
                    )
                    .OrderByDescending(x => x.ImageRuntimeVersion);

                if (assemblies.Any())
                {
                    foreach (var asm in assemblies)
                    {
                        type = asm.GetType(TypeName);

                        if (type is not null)
                            break;
                    }

                    // note, calling GetTypes() will validate the assembly integrity; we may not have retrieved
                    //  the type due to missing references or version conflicts... the GetType() call will simply
                    //  return null while this GetTypes() approach will throw exception if the assembly is invalid...

                    if (type is null)
                    {
                        foreach (var asm in assemblies)
                        {
                            var asmTypes = asm.GetTypes();
                            type = asmTypes.Single(x => x.Name == TypeName);

                            if (type is not null)
                                break;
                        }
                    }
                }
            }

            if (type is null)
                throw new Exception("Could not find type.");

            if (IsGeneric)
            {
                var genericTypes = GenericTypes.Select(x => x.GetDefinedType()).ToArray();
                type = type.MakeGenericType(genericTypes);
            }

            return type;
        }

        /// <summary>
        /// Returns a string representation of the type discriminator.
        /// </summary>
        /// <returns>The string representation of the type discriminator.</returns>
        public override string ToString()
        {
            var result = IsGeneric ? $"{TypeName}[{GenericTypes.Select(a => $"[{a}]").Aggregate((a, b) => $"{a},{b}")}]" : TypeName;
            result += ", " + AssemblyName;

            if (AssemblyVersion is not null)
                result += ", " + AssemblyVersion;

            return result;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Creates a type discriminator from the specified type.
        /// </summary>
        /// <param name="T">The type to create the discriminator from.</param>
        /// <param name="enforceVersion">A flag indicating whether to enforce the version of the assembly.</param>
        /// <returns>The type discriminator.</returns>
        public static TypeDiscriminator From(Type T, bool enforceVersion = false)
        {
            if (T == typeof(string))
                throw new Exception("Use the 'FromString' method of TypeDiscriminator when parsing string values");

            if (T.IsGenericType)
            {
                return new TypeDiscriminator()
                {
                    AssemblyName = T.Assembly.GetName().Name ?? throw new Exception("Could not get assembly name."),
                    TypeName = T.GetGenericTypeDefinition().FullName ?? throw new Exception("Could not get type name."),
                    AssemblyVersion = enforceVersion ? T.Assembly.GetName().Version?.ToString() : null,
                    GenericTypes = T.GetGenericArguments().Select(genericT => TypeDiscriminator.From(genericT, enforceVersion)).ToArray()
                };
            }
            else
            {
                return new TypeDiscriminator()
                {
                    AssemblyName = T.Assembly.GetName().Name ?? throw new Exception("Could not get assembly name."),
                    TypeName = T.FullName ?? throw new Exception("Could not get type name."),
                    AssemblyVersion = enforceVersion ? T.Assembly.GetName().Version?.ToString() : null
                };
            }
        }

        /// <summary>
        /// Creates a type discriminator from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create the discriminator from.</typeparam>
        /// <param name="enforceVersion">A flag indicating whether to enforce the version of the assembly.</param>
        /// <returns>The type discriminator.</returns>
        public static TypeDiscriminator From<T>(bool enforceVersion = false)
        {
            return From(typeof(T), enforceVersion);
        }

        /// <summary>
        /// Creates a type discriminator from the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to create the discriminator from.</param>
        /// <param name="enforceVersion">A flag indicating whether to enforce the version of the assembly.</param>
        /// <returns>The type discriminator.</returns>
        public static TypeDiscriminator From<T>(T obj, bool enforceVersion = false) where T : notnull
        {
            return From(obj.GetType(), enforceVersion);
        }

        /// <summary>
        /// Creates a type discriminator from the specified discriminator string.
        /// </summary>
        /// <param name="discriminatorString">The discriminator string.</param>
        /// <returns>The type discriminator.</returns>
        public static TypeDiscriminator FromString(string discriminatorString)
        {
            // type, assembly, version
            // Sidub.Panel.SystemApp.SystemApplication, Sidub.Panel.SystemApp, 1.0.2.4
            // Sidub.Panel.SystemApp.GenericApplication`1[[Sidub.Panel.SystemApp.SystemApplication, Sidub.Panel.SystemApp, 1.0.1.1],[Sidub.Panel.SystemApp.SystemApplication, Sidub.Panel.OtherApp, 1.0.1.4]], Sidub.Panel.SystemApp, 1.0.2.4

            string[] segments;

            var result = new TypeDiscriminator();

            if (discriminatorString.Contains("`1"))
            {
                // generic
                var typeSplit = discriminatorString.Split(new[] { "`1" }, StringSplitOptions.None);
                var typeName = typeSplit[0] + "`1";
                var typeArgumentString = typeSplit[1].Substring(0, typeSplit[1].LastIndexOf(']')).TrimStart('[');
                var typeArguements = typeArgumentString.Split(new[] { "],[" }, StringSplitOptions.None).Select(x => x.Replace("[", string.Empty).Replace("]", string.Empty));
                result.GenericTypes = typeArguements.Select(FromString).ToArray();

                var remainder = typeSplit[1].Substring(typeSplit[1].LastIndexOf(']') + 3);
                segments = new[] { typeName }.Concat(remainder.Split(',').Select(x => x.Trim())).ToArray();
            }
            else
            {
                // non-generic
                segments = discriminatorString.Split(',').Select(x => x.Trim()).ToArray();
            }

            if (segments.Length < 2)
                throw new Exception("Invalid type discriminator string; minimum two segments (type, assembly) is expected but less than two received.");

            result.AssemblyName = segments[1];
            result.TypeName = segments[0];

            if (segments.Length - 3 >= 0)
            {
                result.AssemblyVersion = segments[2];
            }

            return result;
        }

        #endregion

    }

}
