using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class InformationInput
    {
        public string CorporateAddress { get; set; }
        public List<BilingInformationInput> BilingInformationInputs { get; set; } = new List<BilingInformationInput>();
    }

    public class BilingInformationInput
	{
        public long? Id { get; set; }
        public string? CardholderName { get; set; } = null;
        public string? CardNumber { get; set; } = null;
        public int Expiry { get; set; } = 0;
        public string? CVC { get; set; } = null;
        public string Address { get; set; }
        public string? City { get; set; } = null;
        public string? State { get; set; } = null;
        public string? ZIPCode { get; set; } = null;
        public bool IsPaypal { get; set; } = false;

    }
}
