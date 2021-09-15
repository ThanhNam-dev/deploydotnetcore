using EMa.Data.Entities.Common;
using System;

namespace EMa.Data.Entities
{
    public class Quiz : ModelBase
    {
        public string QuestionName { get; set; }
        public int NoAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public string InCorrectAnswer1 { get; set; }
        public string InCorrectAnswer2 { get; set; }
        public string InCorrectAnswer3 { get; set; }
        public string InCorrectAnswer4 { get; set; }
        public string InCorrectAnswer5 { get; set; }
    }
}
