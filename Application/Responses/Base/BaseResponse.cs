using System;

namespace ScoutDirect.Application.Responses.Base
{
    public class BaseResponse<T>
    {
        protected readonly static string Domain = "https://barber-man.com";
        public T Id { get; set; }
    }
}
