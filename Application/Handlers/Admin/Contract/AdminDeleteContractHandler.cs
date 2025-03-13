using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Commands.Admin;

namespace CMPNatural.Application
{
    public class AdminDeleteContractHandler : IRequestHandler<AdminDeleteContractCommand, CommandResponse<Contract>>
    {
        private readonly IContractRepository _repository;
        private readonly ILocationCompanyRepository _locrepository;
        public AdminDeleteContractHandler(IContractRepository repository, ILocationCompanyRepository locrepository)
        {
            _repository = repository;
            _locrepository = locrepository;
        }
        public async Task<CommandResponse<Contract>> Handle(AdminDeleteContractCommand request, CancellationToken cancellationToken)
        {
            //var isRegistered = (await _locrepository.GetAsync(x=>x.CapacityId == request.Id)).Any();

            //if (isRegistered)
            //{
            //    return new NoAcess<Capacity>() { Message = "This capacity is already in use and cannot be deleted." };
            //}

            var entity = await _repository.GetByIdAsync(request.Id);
            await _repository.DeleteAsync(entity);
            return new Success<Contract>() { Data = entity };
        }
    }
}

