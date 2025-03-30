using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Provider.Biling
{
	public class ProviderAddBilingCommand : BilingInformationInput, IRequest<CommandResponse<BillingInformationProvider>>
    {
		public ProviderAddBilingCommand(BilingInformationInput input,long ProviderId)
		{
            CardholderName = input.CardholderName;
            CardNumber = input.CardNumber;
            Expiry = input.Expiry;
            CVC = input.CVC;
            Address = input.Address;
            City = input.City;
            State = input.State;
            ZIPCode = input.ZIPCode;
            IsPaypal = input.IsPaypal;
            this.ProviderId = ProviderId;
        }
		public long ProviderId { get; set; }
	}
}

