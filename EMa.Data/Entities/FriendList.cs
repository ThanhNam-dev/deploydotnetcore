using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class FriendList : ModelBase
    {
        public Guid FriendId { get; set; }
        public Guid UserId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual FriendShip FriendShip { get; set; }
    }
}
