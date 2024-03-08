#region Imports

using System.ComponentModel;
using System.Reflection;

#endregion

namespace Sidub.Platform.Core.Extensions
{

    /// <summary>
    /// Provides extension methods for working with enums.
    /// </summary>
    public static class EnumExtensions
    {

        #region Public static methods

        /// <summary>
        /// Retrieves the individual flags of an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="e">The enum value.</param>
        /// <returns>An enumerable of individual flags.</returns>
        public static IEnumerable<T> GetFlags<T>(this T e) where T : Enum
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).Cast<T>();
        }

        /// <summary>
        /// Retrieves the description of an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription<T>(this T enumValue)
            where T : Enum
        {
            if (!typeof(T).IsEnum)
                return string.Empty;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo is not null)
            {
                var attr = fieldInfo.GetCustomAttribute<DescriptionAttribute>(true);

                if (attr is not null)
                {
                    description = attr.Description;
                }
            }

            return description;
        }

        #endregion

    }
}
