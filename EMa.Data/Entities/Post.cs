using EMa.Data.Entities.Common;
using System;
using System.Collections.Generic;

namespace EMa.Data.Entities
{
    public class Post : ModelBase
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual List<PostComment> PostComments { get; set; }
    }
}
