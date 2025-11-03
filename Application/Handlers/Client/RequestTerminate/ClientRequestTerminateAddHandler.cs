using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Client;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class ClientRequestTerminateAddHandler : IRequestHandler<ClientRequestTerminateAddCommand, CommandResponse<RequestTerminate>>
    {
        private readonly IRequestTerminateRepository _repository;
        private readonly IRequestRepository _iinvoiceRepository;

        public ClientRequestTerminateAddHandler(IRequestTerminateRepository _repository, IRequestRepository _iinvoiceRepository)
        {
            this._repository = _repository;
            this._iinvoiceRepository = _iinvoiceRepository;
        }

        public async Task<CommandResponse<RequestTerminate>> Handle(ClientRequestTerminateAddCommand request, CancellationToken cancellationToken)
        {
            var isAddedd = (await _repository.GetAsync(x => x.RequestId == request.RequestId &&
            (x.RequestTerminateStatus != RequestTerminateProcessEnum.Updated)
            )).FirstOrDefault();

            if (isAddedd != null)
            {
                return new NoAcess<RequestTerminate>()
                {
                    Message = "A termination request for this invoice has already been submitted."
                };
            }

            var invoice = (await _iinvoiceRepository.GetAsync(x => x.Id == request.RequestId)).FirstOrDefault();

            var entity = new RequestTerminate()
            {
                RequestId = invoice.Id,
                ContractId = invoice.ContractId??0,
                OperationalAddressId = invoice.OperationalAddressId,
                CompanyId = invoice.CompanyId,
                CreatedAt = DateTime.Now,
                Message = request.Message,
                Status = request.Status,
                RequestTerminateNumber = ""
            };

            await _repository.AddAsync(entity);
            entity.RequestTerminateNumber = entity.Number;
            await _repository.UpdateAsync(entity);

            return new Success<RequestTerminate>() { Data = entity };
        }
    }
}

