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

        [HttpPost("createProgram")]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramDto programDto)
        {
            var program = _mapper.Map<ProgramModel>(programDto);
            var result = await _programApplicationRepo.CreateProgram(program);
            return Ok(result);
        }

        [HttpPost("createProgramQuestions")]
        public async Task<IActionResult> CreateQuestions([FromBody] List<QuestionDto> questionDto, string programId)
        {
            if (!questionDto.Any())
                return BadRequest("Question cannot be null");

            var question = _mapper.Map<List<QuestionModel>>(questionDto);
            var result = await _programApplicationRepo.CreateQuestions(programId, question);
            return Ok(result);
        }

        [HttpGet("getProgram/{id}")]
        public async Task<IActionResult> GetProgram(string programId)
        {
            var program = await _programApplicationRepo.GetProgram(programId);

            if (program == null)
                return NotFound();

            var programDto = _mapper.Map<ProgramDto>(program);
            return Ok(programDto);
        }

        [HttpGet("getProgramQuestions/{id}")]
        public async Task<IActionResult> GetProgramQuestions(string programId)
        {
            var programQuestion = await _programApplicationRepo.GetProgramQuestion(programId);

            if (programQuestion == null)
                return NotFound();

            var programQuestionDto = _mapper.Map<List<QuestionDto>>(programQuestion);
            return Ok(programQuestionDto);
        }

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
