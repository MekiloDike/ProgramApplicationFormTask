using ProgramApplicationFormTask.Model;

namespace ProgramApplicationFormTask.IRepository
{
    public interface IProgramApplicationFormRepo
    {
        Task<string> CreateProgram(ProgramModel model);
        Task<bool> CreateQuestions(string programId, List<QuestionModel> model);
        Task<ProgramModel> GetProgram(string programId);
        Task<List<QuestionModel>> GetProgramQuestion(string programId);
        Task<bool> EditProgramQuestions(List<QuestionModel> model, string programId);
        Task<bool> FillProgramApplicationForm(FillApplicationFormModel applicationForm, string programId);
    }
}
