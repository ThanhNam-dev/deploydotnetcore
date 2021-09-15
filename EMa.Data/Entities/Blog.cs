using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Blog : ModelBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public string Thumbnail { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
