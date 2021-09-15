using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class PostComment : ModelBase
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Post Post { get; set; }

    }
}
