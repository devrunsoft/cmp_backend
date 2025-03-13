using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Application.Model;

namespace CMPNatural.Application
{
	public class AdminAddProductCommand : ProductInput, IRequest<CommandResponse<Product>>
    {
        public AdminAddProductCommand(ProductInput input)
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
        }
	}
}

