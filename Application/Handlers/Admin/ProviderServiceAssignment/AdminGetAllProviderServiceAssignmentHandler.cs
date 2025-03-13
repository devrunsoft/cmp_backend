using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses.ProviderServiceAssignment;

namespace CMPNatural.Application
{

    public class AdminGetAllProviderServiceAssignmentHandler : IRequestHandler<AdminGetAllProviderServiceAssignmentCommand,
        CommandResponse<PagesQueryResponse<BaseServiceAppointmentResponse>>>
    {
        private readonly IBaseServiceAppointmentRepository _providerService;

        public AdminGetAllProviderServiceAssignmentHandler(IBaseServiceAppointmentRepository providerService)
        {
            _providerService = providerService;
        }

        public async Task<CommandResponse<PagesQueryResponse<BaseServiceAppointmentResponse>>> Handle(AdminGetAllProviderServiceAssignmentCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _providerService.GetBasePagedAsync(request, p => p.ProviderId != null && (request.providerId==null? true: (p.ProviderId == request.providerId))
            ,query=> query.Include(x=>x.Invoice).ThenInclude(x=>x.Provider)
            .Include(x => x.Invoice).ThenInclude(x=>x.Company)
            ));

            var model = new PagesQueryResponse<BaseServiceAppointmentResponse>(
                invoices.elements.Select(p => BaseServiceAppointmentMapper.Mapper.Map<BaseServiceAppointmentResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<BaseServiceAppointmentResponse>>() { Data = model };

        }

    }
}

