namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents a signature entity field.
    /// </summary>
    /// <remarks>
    /// This field is used to digitally sign an entity.
    /// </remarks>
    public record SignatureEntityField : IEntityField
    {

        #region Member variables

        private static SignatureEntityField? _instance = null;
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
        public string FieldName => "__sidub_entitySignature";

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public Type FieldType => typeof(byte[]);

        /// <summary>
        /// Gets the label of the field.
        /// </summary>
        public string Label => "Signature";

        /// <summary>
        /// Gets the ordinal position of the field.
        /// </summary>
        public int OrdinalPosition => -1;

        #endregion

        #region Static methods

        /// <summary>
        /// Gets the singleton instance of the <see cref="TypeDiscriminatorEntityField"/> class.
        /// </summary>
        public static SignatureEntityField Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance is null)
                        {
                            _instance = new SignatureEntityField();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

    }
}
