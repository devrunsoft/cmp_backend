using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminTermsConditionsDeleteCommand : IRequest<CommandResponse<TermsConditions>>
    {
		public AdminTermsConditionsDeleteCommand()
		{
		}
        public long Id { get; set; }
    }
}

