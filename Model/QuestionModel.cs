using ProgramApplicationFormTask.Utility;

namespace ProgramApplicationFormTask.Model
{
    public class QuestionModel
    {
        public string Id { get; set; }
        public QuestionType Question { get; set; }
        public bool HasOtherOption { get; set; }
    }
}
