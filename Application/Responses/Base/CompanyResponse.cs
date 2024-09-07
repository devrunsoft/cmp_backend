using Barbara.Application.Responses.Base;
using ScoutDirect.Application.Responses.Base;
using System;

namespace CMPNatural.Application.Responses
{
    public class CompanyResponse : BaseResponse<Guid>
    {
        public long? Id { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryPhonNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string Position { get; set; }
        public string SecondaryFirstName { get; set; }
        public string SecondaryLastName { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string ReferredBy { get; set; }
        public string AccountNumber { get; set; }
        public bool Registered { get; set; }
        public bool Accepted { get; set; }
        public int Type { get; set; }


    }
}
