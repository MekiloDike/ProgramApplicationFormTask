using ProgramApplicationFormTask.IRepository;
using ProgramApplicationFormTask.Repository;
using ProgramApplicationFormTask.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mapper = builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddSingleton<IProgramApplicationFormRepo>(InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDbConnection")).GetAwaiter().GetResult());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
static async Task<ProgramApplicationFormRepo> InitializeCosmosClientInstanceAsync(IConfigurationSection config)
{
    var databaseName = config["DatabaseName"];
    var ContainerNameCreateProgram = config["Container_CreateProgram"];
    var containerNameCreateQuestion = config["Container_CreateQuestions"];
    var containerNameFilledForms = config["Container_CreateFormcontainerNameFilledForms"];
    var url = config["Url"];
    var key = config["PrimaryKey"];
    var client = new Microsoft.Azure.Cosmos.CosmosClient(url, key);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(ContainerNameCreateProgram, "/id");
    await database.Database.CreateContainerIfNotExistsAsync(containerNameCreateQuestion, "/id");
    await database.Database.CreateContainerIfNotExistsAsync(containerNameFilledForms, "/id");
    return new ProgramApplicationFormRepo(client, databaseName, ContainerNameCreateProgram, containerNameCreateQuestion, containerNameFilledForms);
}
