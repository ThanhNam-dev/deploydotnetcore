using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class FriendShip : ModelBase
    {
        public Guid SenderId { get; set; }
        public Guid ReceipId { get; set; }
        public bool Confirmed { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
