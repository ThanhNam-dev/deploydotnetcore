using EMa.Data.Entities.Common;

namespace EMa.Data.ViewModel
{
    public class ListQuizViewModel : ModelBase
    {
        public string QuestionName { get; set; }
        public int NoAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public string InCorrectAnswer1 { get; set; }
        public string InCorrectAnswer2 { get; set; }
        public string InCorrectAnswer3 { get; set; }
        public string InCorrectAnswer4 { get; set; }
        public string InCorrectAnswer5 { get; set; }
        public string LessonName { get; set; }
        public string QuizTypeName { get; set; }
    }
}
