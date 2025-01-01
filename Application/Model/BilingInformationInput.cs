using System;
namespace CMPNatural.Application.Model
{
	public class BilingInformationInput
	{
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

