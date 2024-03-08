namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents a type discriminator entity field.
    /// </summary>
    /// <remarks>
    /// This field is used to differentiate between different types of entities.
    /// </remarks>
    public record TypeDiscriminatorEntityField : IEntityField
    {

        #region Member variables

        private static TypeDiscriminatorEntityField? _instance = null;
        private static readonly object _instanceLock = new object();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a value indicating whether this field is a key field.
        /// </summary>
        public bool IsKeyField => false;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string FieldName => "__sidub_entityType";

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public Type FieldType => typeof(string);

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        public string Label => "Type discriminator";

        #endregion

        #region Static methods

        /// <summary>
        /// Gets the singleton instance of the <see cref="TypeDiscriminatorEntityField"/> class.
        /// </summary>
        public static TypeDiscriminatorEntityField Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance is null)
                        {
                            _instance = new TypeDiscriminatorEntityField();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

    }
}
