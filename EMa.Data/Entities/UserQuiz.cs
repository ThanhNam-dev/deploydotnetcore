using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class UserQuiz : ModelBase
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Answer { get; set; }
        public bool RightOrWrong { get; set; }
        public int NoExams { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
