using ProgramApplicationFormTask.Utility;

namespace ProgramApplicationFormTask.Model
{
    public class QuestionModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool HasOtherOption { get; set; }
    }
}
