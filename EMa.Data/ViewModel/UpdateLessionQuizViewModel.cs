using System;

namespace EMa.Data.ViewModel
{
    public class UpdateLessionQuizViewModel
    {
        public string QuestionType { get; set; }
        public Guid LesionId { get; set; }
        public Guid QuizId { get; set; }
    }
}
