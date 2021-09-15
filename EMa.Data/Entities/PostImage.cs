using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class PostImage : ModelBase
    {
        public Guid ImageId { get; set; }
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Image Image { get; set; }
    }
}
