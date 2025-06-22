using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Entities.Base;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class AdminProcessingManifestHandler : IRequestHandler<AdminProcessingManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;
        public AdminProcessingManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }
        public async Task<CommandResponse<Manifest>> Handle(AdminProcessingManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.Id && x.Status == ManifestStatus.Scaduled)).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<Manifest>
                {
                    Message = "This manifest has already been assigned to a provider and cannot be reassigned."
                };
            }


            await _repository.UpdateAsync(result);
            return new Success<Manifest>() { Data = result };
        }
    }
}

