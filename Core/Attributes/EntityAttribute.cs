#region Imports


#endregion

namespace Sidub.Platform.Core.Attributes
{

    /// <summary>
    /// Represents an attribute that can be applied to classes or interfaces to mark them as entities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class EntityAttribute : Attribute
    {

        #region Public properties

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entity is versioned.
        /// </summary>
        public bool IsVersioned { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttribute"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityAttribute(string name)
        {
            Name = name;
            IsVersioned = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAttribute"/> class with the specified name and versioned flag.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="isVersioned">A value indicating whether the entity is versioned.</param>
        public EntityAttribute(string name, bool isVersioned)
        {
            Name = name;
            IsVersioned = isVersioned;
        }

        #endregion

    }
}
