using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class AdminTermsConditionsPaginateCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<TermsConditions>>>
    {
		public AdminTermsConditionsPaginateCommand()
		{
		}

		public bool? Enable { get; set; }
        public TermsConditionsEnum? Type { get; set; }
    }
}

