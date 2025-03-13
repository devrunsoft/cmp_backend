using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Xml.Linq;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Commands
{
    public class AdminUpdateProductPriceCommand : ProductPriceInput, IRequest<CommandResponse<ProductPrice>>
    {
        public AdminUpdateProductPriceCommand(ProductPriceInput input , long Id)
        {
            Name = input.Name;
            Amount = input.Amount;
            MinimumAmount = input.MinimumAmount;
            BillingPeriod = input.BillingPeriod;
            NumberofPayments = input.NumberofPayments;
            SetupFee = input.SetupFee;
            Enable = input.Enable;
            Order = input.Order;
            this.Id = Id;
        }
        public long Id { get; set; }
    }
}

