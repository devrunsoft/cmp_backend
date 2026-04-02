using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands.Provider.Driver;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class DeleteDriverOfProviderHandler : IRequestHandler<DeleteDriverOfProviderCommand, CommandResponse<DriverResponse>>
    {
        private readonly IProviderDriverRepository _repository;
        public DeleteDriverOfProviderHandler(IProviderDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(DeleteDriverOfProviderCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.DriverId == request.DriverId && p.ProviderId == request.ProviderId, query=> query.Include(x=>x.Driver))).FirstOrDefault();
            await _repository.DeleteAsync(result);
            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(result.Driver) };
        }
    }
}

