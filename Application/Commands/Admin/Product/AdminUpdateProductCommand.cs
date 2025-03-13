using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateProductCommand : ProductInput, IRequest<CommandResponse<Product>>
    {
        public AdminUpdateProductCommand(ProductInput input, long Id)
        {
            Name = input.Name;
            Description = input.Description;
            ProductType = input.ProductType;
            Type = input.Type;
            ServiceType = input.ServiceType;
            //IsEmergency = input.IsEmergency;
            CollectionIds = input.CollectionIds;
            ServiceCrmId = input.ServiceCrmId;
            Enable = input.Enable;
            Order = input.Order;
            this.Id = Id;
        }
        public long Id { get; set; }
	}
}