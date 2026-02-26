using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminAddContractCommand : ContractInput, IRequest<CommandResponse<Contract>>
    {
		public AdminAddContractCommand(ContractInput input)
		{
			Active = input.Active;
			Content = input.Content;
            Title = input.Title;
			IsDefault = input.IsDefault;
			Type = input.Type;
        }
	}
}

