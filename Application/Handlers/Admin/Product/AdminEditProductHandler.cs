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
    public class AdminEditProductHandler : IRequestHandler<AdminUpdateProductCommand, CommandResponse<Product>>
    {
        private readonly IProductRepository _repository;
        public AdminEditProductHandler(IProductRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<Product>> Handle(AdminUpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            entity.Enable = request.Enable;
            entity.Name = request.Name;
            entity.ServiceType = (int)request.ServiceType;
            entity.Description = request.Description;
            entity.Type = (int)request.Type;
            entity.ProductType = request.ProductType;
            entity.Order = request.Order;
            //entity.IsEmergency = request.IsEmergency;

            await _repository.UpdateAsync(entity);
            return new Success<Product>() { Data = entity };
        }
    }
}
