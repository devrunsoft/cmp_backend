using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Xml.Linq;

namespace CMPNatural.Application
{
    public class AdminAddProductPriceCommand : ProductPriceInput, IRequest<CommandResponse<ProductPrice>>
    {
        public AdminAddProductPriceCommand(ProductPriceInput input, long ProductId)
        {
            Name = input.Name;
            Amount = input.Amount;
            MinimumAmount = input.MinimumAmount;
            BillingPeriod = input.BillingPeriod;
            NumberofPayments = input.NumberofPayments;
            SetupFee = input.SetupFee;
            Enable = input.Enable;
            Order = input.Order;
            this.ProductId = ProductId;
        }
        public long ProductId { get; set; }
    }
}

