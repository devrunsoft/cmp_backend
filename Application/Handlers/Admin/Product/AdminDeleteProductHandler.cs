using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminDeleteProductHandler : IRequestHandler<AdminDeleteProductCommand, CommandResponse<Product>>
    {
        private readonly IProductRepository _repository;
        private readonly IProductPriceRepository _repositoryPrice;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        public AdminDeleteProductHandler(IProductRepository providerReposiotry ,
            IProductPriceRepository repositoryPrice
            , IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _repositoryPrice = repositoryPrice;
            _repository = providerReposiotry;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }
        public async Task<CommandResponse<Product>> Handle(AdminDeleteProductCommand request, CancellationToken cancellationToken)
        {
            var isRegistered = (await _baseServiceAppointmentRepository.GetAsync(x => x.ProductId == request.Id)).Any();
            if (isRegistered)
            {
                return new NoAcess<Product>() { Message = "This product is already in use and cannot be deleted." };
            }

            var entity = await _repository.GetByIdAsync(request.Id);

            var entityPrice = (await _repositoryPrice.GetAsync(x=>x.ProductId == request.Id)).ToList();
            await _repository.DeleteAsync(entity);
            await _repositoryPrice.DeleteRangeAsync(entityPrice);
            return new Success<Product>() { Data = entity };
        }
    }
}
