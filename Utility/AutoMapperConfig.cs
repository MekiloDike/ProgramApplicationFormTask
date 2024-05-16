using AutoMapper;
using ProgramApplicationFormTask.Dto;
using ProgramApplicationFormTask.Model;

namespace ProgramApplicationFormTask.Utility
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<QuestionDto, QuestionModel>().ReverseMap();
            CreateMap<ProgramDto, ProgramModel>().ReverseMap();
            CreateMap<FillApplicationFormDto, FillApplicationFormModel>();
        }
    }
}
