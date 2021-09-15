using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Notification : ModelBase
    {
        public string Content { get; set; }
        public Guid? UserId { get; set; }
        public string Type { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
