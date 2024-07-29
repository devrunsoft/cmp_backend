using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class BenchmarkMappingProfile : Profile
    {
        public BenchmarkMappingProfile()
        {
            CreateMap<BenchMark, BenchmarkResponse>().ReverseMap();
        }
    }
}
