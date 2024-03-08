namespace Sidub.Platform.Core.Entity.Relations
{

    /// <summary>
    /// Represents a list of entity references.
    /// </summary>
    public interface IEntityReferenceList : IEnumerable<IEntityReference>
    {

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        internal Type EntityType { get; }

        /// <summary>
        /// Gets the references that have been removed from the list.
        /// </summary>
        IEnumerable<IEntityReference> RemovedReferences { get; }

        /// <summary>
        /// Commits the changes made to the entity reference list.
        /// </summary>
        public void Commit();

    }

}
