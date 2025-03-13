using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminDeleteCapacityHandler : IRequestHandler<AdminDeleteCapacityCommand, CommandResponse<Capacity>>
    {
        private readonly ICapacityRepository _repository;
        private readonly ILocationCompanyRepository _locrepository;
        public AdminDeleteCapacityHandler(ICapacityRepository repository, ILocationCompanyRepository locrepository)
        {
            _repository = repository;
            _locrepository = locrepository;
        }
        public async Task<CommandResponse<Capacity>> Handle(AdminDeleteCapacityCommand request, CancellationToken cancellationToken)
        {
            var isRegistered = (await _locrepository.GetAsync(x=>x.CapacityId == request.Id)).Any();

            if (isRegistered)
            {
                return new NoAcess<Capacity>() { Message = "This capacity is already in use and cannot be deleted." };
            }

            var entity = await _repository.GetByIdAsync(request.Id);
            await _repository.DeleteAsync(entity);
            return new Success<Capacity>() { Data = entity };
        }
    }
}

