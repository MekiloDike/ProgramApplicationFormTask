using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProgramApplicationFormTask.Dto;
using ProgramApplicationFormTask.IRepository;
using ProgramApplicationFormTask.Model;

namespace ProgramApplicationFormTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramApplicationFormController : ControllerBase
    {
        private readonly IProgramApplicationFormRepo _programApplicationRepo;
        private readonly IMapper _mapper;

        public ProgramApplicationFormController(IProgramApplicationFormRepo programApplicationRepo, IMapper mapper)
        {
            _programApplicationRepo = programApplicationRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new program.
        /// </summary>
        /// <param name="programDto">The DTO containing the program data.</param>
        /// <returns>The result of the program creation operation.</returns>
        [HttpPost("createProgram")]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramDto programDto)
        {
            var program = _mapper.Map<ProgramModel>(programDto);
            var result = await _programApplicationRepo.CreateProgram(program);
            return Ok(result);
        }

        /// <summary>
        /// Creates questions for a program.
        /// </summary>
        /// <param name="questionDto">The DTOs containing the questions data.</param>
        /// <param name="programId">The identifier of the program.</param>
        /// <returns>The result of the question creation operation.</returns>
        [HttpPost("createProgramQuestions")]
        public async Task<IActionResult> CreateQuestions([FromBody] List<QuestionDto> questionDto, string programId)
        {
            if (!questionDto.Any())
                return BadRequest("Question cannot be null");

            var question = _mapper.Map<List<QuestionModel>>(questionDto);
            var result = await _programApplicationRepo.CreateQuestions(programId, question);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a program by its unique identifier.
        /// </summary>
        /// <param name="programId">The unique identifier of the program.</param>
        /// <returns>The program information if found, or a 404 Not Found response if the program is not found.</returns>
        [HttpGet("getProgram/{id}")]
        public async Task<IActionResult> GetProgram(string programId)
        {
            var program = await _programApplicationRepo.GetProgram(programId);

            if (program == null)
                return NotFound();

            var programDto = _mapper.Map<ProgramDto>(program);
            return Ok(programDto);
        }

        /// <summary>
        /// Retrieves questions for a program by its unique identifier.
        /// </summary>
        /// <param name="programId">The unique identifier of the program.</param>
        /// <returns>The list of questions for the program if found, or a 404 Not Found response if the program is not found.</returns>

        [HttpGet("getProgramQuestions/{id}")]
        public async Task<IActionResult> GetProgramQuestions(string programId)
        {
            var programQuestion = await _programApplicationRepo.GetProgramQuestion(programId);

            if (programQuestion == null)
                return NotFound();

            var programQuestionDto = _mapper.Map<List<QuestionDto>>(programQuestion);
            return Ok(programQuestionDto);
        }

        /// <summary>
        /// Edits questions for a program.
        /// </summary>
        /// <param name="questionDto">The DTOs containing the questions data to be edited.</param>
        /// <param name="programId">The identifier of the program.</param>
        /// <returns>The result of the question editing operation.</returns>
        [HttpPut("editProgramQuestions/{id}")]
        public async Task<IActionResult> EditProgramApplicationForm([FromBody] List<QuestionDto> questionDto, string programId)
        {
            if (!questionDto.Any())
                return BadRequest("Question cannot be null");

            //check if the program still exists
            var existingQuestions = await _programApplicationRepo.GetProgramQuestion(programId);
            if (!existingQuestions.Any())
                return NotFound();

            var model = _mapper.Map<List<QuestionModel>>(questionDto);
            var result = await _programApplicationRepo.EditProgramQuestions(model, programId);
            if (!result)
                return StatusCode(500);

            return Ok(result);
        }

        /// <summary>
        /// Fills and submits a program application form.
        /// </summary>
        /// <param name="programApplicationForm">The DTO containing the filled program application form data.</param>
        /// <param name="programId">The identifier of the program.</param>
        /// <returns>The result of the program application form submission operation.</returns>
        [HttpPost("fillAndSubmitForm")]
        public async Task<IActionResult> FillandSubmitProgramApplicationForm([FromBody] FillApplicationFormDto programApplicationForm, string programId)
        {
            var applicationForm = _mapper.Map<FillApplicationFormModel>(programApplicationForm);
            var result = await _programApplicationRepo.FillProgramApplicationForm(applicationForm, programId);
            if (!result)
                return StatusCode(500);
            return Ok(result);
        }
    }
}
