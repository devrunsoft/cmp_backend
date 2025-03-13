using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminDeleteProductPriceHandler : IRequestHandler<AdminDeleteProductPriceCommand, CommandResponse<ProductPrice>>
    {
        private readonly IProductPriceRepository _repository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        public AdminDeleteProductPriceHandler(IProductPriceRepository repository, IBaseServiceAppointmentRepository baseServiceAppointmentRepository)
        {
            _repository = repository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<ProductPrice>> Handle(AdminDeleteProductPriceCommand request, CancellationToken cancellationToken)
        {
            var isRegistered = (await _baseServiceAppointmentRepository.GetAsync(x => x.ProductId == request.Id)).Any();
            if (isRegistered)
            {
                return new NoAcess<ProductPrice>() { Message = "This product is already in use and cannot be deleted." };
            }

            var entity = await _repository.GetByIdAsync(request.Id);
            await _repository.DeleteAsync(entity);
            return new Success<ProductPrice>() { Data = entity };
        }
    }
}

