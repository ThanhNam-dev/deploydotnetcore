using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Lession : ModelBase
    {
        public string Name { get; set; }
        public Guid QuizTypeId { get; set; }
    }
}
