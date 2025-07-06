using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;

namespace CMPNatural.Application.Responses.Information
{
	public class InfromationResponse
	{
		public string CorporateAddress { get; set; }
		public List<BillingInformation> billingInformation { get; set; }
	}
}

