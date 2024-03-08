namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents the action type for an entity relation.
    /// </summary>
    public enum EntityRelationActionType
    {
        /// <summary>
        /// No action.
        /// </summary>
        None,

        /// <summary>
        /// Create action.
        /// </summary>
        Create,

        /// <summary>
        /// Update action.
        /// </summary>
        Update,

        /// <summary>
        /// Delete action.
        /// </summary>
        Delete
    }

    /// <summary>
    /// Represents the load type for an entity relation.
    /// </summary>
    public enum EntityRelationLoadType
    {
        /// <summary>
        /// Unknown load type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Eager load type.
        /// </summary>
        Eager,

        /// <summary>
        /// Lazy load type.
        /// </summary>
        Lazy,

        /// <summary>
        /// Join load type.
        /// </summary>
        Join
    }

    /// <summary>
    /// Represents the relationship type for an entity relation.
    /// </summary>
    public enum EntityRelationshipType
    {
        /// <summary>
        /// Unknown relationship type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Association relationship type.
        /// </summary>
        Association,

        /// <summary>
        /// Composition relationship type.
        /// </summary>
        Composition,

        /// <summary>
        /// Aggregation relationship type.
        /// </summary>
        Aggregation
    }

}
