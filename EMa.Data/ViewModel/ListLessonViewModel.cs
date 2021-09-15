using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.ViewModel
{
    public class ListLessonViewModel : ModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string QuizName { get; set; }
        public Guid QuizTypeId { get; set; }
    }
}
