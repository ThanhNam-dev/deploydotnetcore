using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Message : ModelBase
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
