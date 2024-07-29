using Bazaro.Application.Responses.Base;

namespace Bazaro.Application.Responses
{
    public class BenchmarkResponse : BaseResponse<int>
    {
        public string Labels { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string Descrtiption { get; set; }
    }
}
