using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminAddProductHandler : IRequestHandler<AdminAddProductCommand, CommandResponse<Product>>
    {
        private readonly IProductRepository _repository;
        public AdminAddProductHandler(IProductRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<Product>> Handle(AdminAddProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Enable = request.Enable,
                Name = request.Name,
                ServiceType = (int)request.ServiceType,
                Description = request.Description,
                Type = (int)request.Type,
                ProductType = request.ProductType,
                IsEmergency = request.IsEmergency,
            };

            var result = await _repository.AddAsync(entity);
            return new Success<Product>() { Data = result };
        }
    }
}
