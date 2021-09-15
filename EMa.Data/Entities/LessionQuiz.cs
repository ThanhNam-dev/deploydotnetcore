using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class LessionQuiz : ModelBase
    {
        public string QuestionType { get; set; }
        public Guid LesionId { get; set; }
        public Guid QuizId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Lession Lession { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
