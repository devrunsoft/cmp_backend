using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminEditContractCommand : ContractInput , IRequest<CommandResponse<Contract>>
    {
        public AdminEditContractCommand(ContractInput input, long Id)
        {
            Active = input.Active;
            Content = input.Content;
            Title = input.Title;
            this.Id = Id;
            IsDefault = input.IsDefault;
            Type = input.Type;
        }

        public long Id { get; set; }
    }
}

