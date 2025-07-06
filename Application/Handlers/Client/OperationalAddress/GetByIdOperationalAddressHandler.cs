using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{
    public class GetByIdOperationalAddressHandler : IRequestHandler<GetByIdServiceOperationalAddressCommand, CommandResponse<OperationalAddress>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public GetByIdOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<OperationalAddress>> Handle(GetByIdServiceOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            OperationalAddress result = (await _operationalAddressRepository.GetAsync(p => p.CompanyId == request.CompanyId && p.Id==request.Id,
                query=> query.Include(p=>p.LocationCompany).Include(x=>x.LocationDateTimes)
                )).FirstOrDefault();

            return new Success<OperationalAddress>() { Data = result };
        }
    }
}
