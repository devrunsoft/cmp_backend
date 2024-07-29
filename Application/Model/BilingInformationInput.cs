using System;
namespace CMPNatural.Application.Model
{
	public class BilingInformationInput
	{
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public int Expiry { get; set; }
        public string CVC { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIPCode { get; set; }
        public bool IsPaypal { get; set; }

    }
}

