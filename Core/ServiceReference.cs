namespace Sidub.Platform.Core
{

    /// <summary>
    /// Represents a service reference.
    /// </summary>
    public abstract record class ServiceReference
    {

        /// <summary>
        /// Gets the name of the service reference.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceReference"/> class.
        /// </summary>
        /// <param name="name">The name of the service reference.</param>
        public ServiceReference(string name)
        {
            Name = name;
        }

    }

}
