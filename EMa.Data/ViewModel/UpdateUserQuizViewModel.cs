using System;

namespace EMa.Data.ViewModel
{
    public class UpdateUserQuizViewModel
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Answer { get; set; }
        public bool RightOrWrong { get; set; }
        public int NoExams { get; set; }
    }
}
