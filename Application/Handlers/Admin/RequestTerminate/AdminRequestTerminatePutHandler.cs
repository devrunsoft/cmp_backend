using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Command;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminRequestTerminatePutHandler : IRequestHandler<AdminRequestTerminatePutCommand, CommandResponse<RequestTerminate>>
    {
        private readonly IRequestTerminateRepository terminateRepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;

        public AdminRequestTerminatePutHandler(IinvoiceRepository invoiceRepository, IManifestRepository _manifestRepository, IRequestTerminateRepository terminateRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._manifestRepository = _manifestRepository;
            this.terminateRepository = terminateRepository;
        }

        public async Task<CommandResponse<RequestTerminate>> Handle(AdminRequestTerminatePutCommand request, CancellationToken cancellationToken)
        {
            var terminaterequest = (await terminateRepository.GetAsync(x => x.Id == request.Id)).FirstOrDefault();

            terminaterequest.RequestTerminateStatus = RequestTerminateProcessEnum.Updated;
            await terminateRepository.UpdateAsync(terminaterequest);

            return new Success<RequestTerminate>() { Data = terminaterequest };
        }
    }
}

