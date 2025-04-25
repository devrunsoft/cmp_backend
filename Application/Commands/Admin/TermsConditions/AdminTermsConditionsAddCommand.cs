using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminTermsConditionsAddCommand : TermsConditionsInput, IRequest<CommandResponse<TermsConditions>>
    {
		public AdminTermsConditionsAddCommand(TermsConditionsInput input)
		{
			this.Type = input.Type;
			this.Content = input.Content;
			this.Enable = input.Enable;
		}
	}
}

