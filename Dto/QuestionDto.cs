using ProgramApplicationFormTask.Utility;

namespace ProgramApplicationFormTask.Dto
{
    public class QuestionDto
    {
        public QuestionType Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool HasOtherOption { get; set; }
    }
}
