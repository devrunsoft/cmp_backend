using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class TermsConditionsInput
	{
        public TermsConditionsEnum Type { get; set; }
        public bool Enable { get; set; }
        public string Content { get; set; } = "";
    }
}

