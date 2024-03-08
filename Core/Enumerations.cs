namespace Sidub.Platform.Core
{

    /// <summary>
    /// Represents the types of fields in an entity.
    /// </summary>
    [Flags]
    public enum EntityFieldType
    {
        /// <summary>
        /// No fields.
        /// </summary>
        None = 0,

        /// <summary>
        /// Key fields.
        /// </summary>
        Keys = 1,

        /// <summary>
        /// Regular fields.
        /// </summary>
        Fields = 2,

        /// <summary>
        /// All fields.
        /// </summary>
        All = Keys | Fields
    }

}
