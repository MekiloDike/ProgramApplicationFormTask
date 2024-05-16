using Microsoft.Azure.Cosmos;
using ProgramApplicationFormTask.IRepository;
using ProgramApplicationFormTask.Model;
using System.Net;

namespace ProgramApplicationFormTask.Repository
{
    public class ProgramApplicationFormRepo : IProgramApplicationFormRepo
    {
        private readonly Container _containerCreateProgram;
        private readonly Container _containerCreateQuestions;
        private readonly Container _containerFilledForm;

        public ProgramApplicationFormRepo(CosmosClient cosmosClient, string databaseName, string containerCreateProject, string containerCreateQuestions, string containerFilledForm)
        {
            _containerCreateProgram = cosmosClient.GetContainer(databaseName, containerCreateProject);
            _containerCreateQuestions = cosmosClient.GetContainer(databaseName, containerCreateQuestions);
            _containerFilledForm = cosmosClient.GetContainer(databaseName, containerFilledForm);
        }
        public async Task<string> CreateProgram(ProgramModel model)
        {
            try
            {
                var response = await _containerCreateProgram.CreateItemAsync(model, new PartitionKey(model.Id));
                return response.Resource.Id;

            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB exception: Status code {ex.StatusCode}, Message: {ex.Message}");
                return ex.Message;
            }
        }
        public async Task<bool> CreateQuestions(string programId, List<QuestionModel> model)
        {
            try
            {
                foreach (var question in model)
                {
                    question.ProgramId = programId;
                    var response = await _containerCreateQuestions.CreateItemAsync(model, new PartitionKey(programId));
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        // If creation of any item fails, return false 
                        return false;
                    }
                }
                return true;

            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB exception: Status code {ex.StatusCode}, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<List<QuestionModel>> GetProgramQuestion(string programId)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.ProgramId = @programId")
                    .WithParameter("@programId", programId);

                var questionList = new List<QuestionModel>();

                using (FeedIterator<QuestionModel> resultSetIterator = _containerCreateQuestions.GetItemQueryIterator<QuestionModel>(query))
                {
                    while (resultSetIterator.HasMoreResults)
                    {
                        FeedResponse<QuestionModel> response = await resultSetIterator.ReadNextAsync();
                        questionList.AddRange(response);
                    }
                }

                return questionList;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Cosmos DB exception: Status code {ex.StatusCode}, Message: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> EditProgramQuestions(List<QuestionModel> model, string programId)
        {
            try
            {
                foreach (var question in model)
                {
                    var response = await _containerCreateQuestions.UpsertItemAsync(question, new PartitionKey(programId));

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        // If creation of any item fails, return false 
                        return false;
                    }
                }
                // If all updates are successful, return true
                return true;
            }
            catch (CosmosException)
            {
                return false;
            }
        }

        public async Task<bool> FillProgramApplicationForm(FillApplicationFormModel applicationForm, string programId)
        {
            try
            {
                var response = await _containerFilledForm.CreateItemAsync(applicationForm, new PartitionKey(programId));
                return response.StatusCode == HttpStatusCode.Created;

            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB exception: Status code {ex.StatusCode}, Message: {ex.Message}");
                return false;
            }
        }

        public async Task<ProgramModel> GetProgram(string programId)
        {
            try
            {
                var response = await _containerCreateProgram.ReadItemAsync<ProgramModel>(programId, new PartitionKey(programId));
                return response.Resource;
            }
            catch (CosmosException)
            {
                return null;
            }
        }
    }
}
