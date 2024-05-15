using ProgramApplicationFormTask.Model;

namespace ProgramApplicationFormTask.Dto
{
    public class FillApplicationFormDto
    {
        public string ProgramId { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public string CurrentResidence { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public List<QuestionSegment>? QuestionSegment { get; set; }
    }

}
