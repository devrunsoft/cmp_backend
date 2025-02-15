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

    public class AdminGetAllProviderServiceAssignmentHandler : IRequestHandler<AdminGetAllProviderServiceAssignmentCommand, CommandResponse<PagesQueryResponse<ProviderServiceAssignmentResponse>>>
    {
        private readonly IProviderServiceAssignmentRepository _providerService;

        public AdminGetAllProviderServiceAssignmentHandler(IProviderServiceAssignmentRepository providerService)
        {
            _providerService = providerService;
        }

        public async Task<CommandResponse<PagesQueryResponse<ProviderServiceAssignmentResponse>>> Handle(AdminGetAllProviderServiceAssignmentCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _providerService.GetBasePagedAsync(request, p => (request.providerId==null?true: p.ProviderId==request.providerId) ,
                query => query.Include(i => i.Company).Include(i => i.Invoice)));

            var model = new PagesQueryResponse<ProviderServiceAssignmentResponse>(
                invoices.elements.Select(p => ProviderServiceAssignmentMapper.Mapper.Map<ProviderServiceAssignmentResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<ProviderServiceAssignmentResponse>>() { Data = model };

        }

    }
}

