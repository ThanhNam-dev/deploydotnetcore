using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Image : ModelBase
    {
        public Guid Id { get; set; }
        public string File { get; set; }
    }
}
