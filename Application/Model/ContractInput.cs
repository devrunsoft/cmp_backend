using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class ContractInput
	{
		public ContractInput()
		{
		}

		public string Content { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public bool IsDefault { get; set; }
        public ContractType Type { get; set; }
    }
}

