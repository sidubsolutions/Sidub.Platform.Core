#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel")]
    internal class TestModel : IEntity
    {

        #region Properties

        [EntityKey<Guid>("Id")]
        internal Guid Id { get; set; } = default;

        [EntityField<string>("ModelDescription")]
        internal string Description { get; set; } = default!;

        [EntityField<int>("Counter")]
        internal int Counter { get; set; } = default;

        public bool IsRetrievedFromStorage { get; set; } = false;

        #endregion

    }

}
