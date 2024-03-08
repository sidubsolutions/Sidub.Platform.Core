#region Imports

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;

#endregion

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("TestModel02")]
    internal class TestModel02 : IEntity
    {

        #region Properties

        [EntityKey<Guid>("Id")]
        internal Guid Id { get; set; } = default;

        [EntityField<string>("ModelDescription")]
        internal string Description { get; set; } = default!;

        [EntityField<int>("Counter")]
        internal int Counter { get; set; } = default;

        [EntityField<Uri>("WebsiteUri")]
        internal Uri? WebsiteUri { get; set; }

        public bool IsRetrievedFromStorage { get; set; } = false;

        #endregion

    }

}
