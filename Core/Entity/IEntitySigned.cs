using Sidub.Platform.Core.Attributes;

namespace Sidub.Platform.Core.Entity
{

    /// <summary>
    /// Represents a signed entity.
    /// </summary>
    public interface IEntitySigned : IEntity
    {

        /// <summary>
        /// Gets or sets the signature of the entity.
        /// </summary>
        [EntityField<byte[]>("__sidub_entitySignature")]
        public byte[]? Signature { get; set; }

    }

}