using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetCustomerProfileStatusHandler : IRequestHandler<SetCustomerProfileStatusCommand, CommandResponse>
    {
        private readonly IBlockedRepository _blockedRepository;

        public SetCustomerProfileStatusHandler(IBlockedRepository blockedRepository)
        {
            _blockedRepository = blockedRepository;
        }

        public async Task<CommandResponse> Handle(SetCustomerProfileStatusCommand request, CancellationToken cancellationToken)
        {
            var Result = await _blockedRepository.GetBlockedAsync(request.Id, (int)request.ShopId);

            if (request.Blocked && Result == null)
            {
                await _blockedRepository.AddAsync(new Blocked() { ShopId = (int)request.ShopId, CustomerId = request.Id });
            }
            else if (Result != null)
            {
                await _blockedRepository.DeleteAsync(Result);
            }

            return new Success() { };
        }

    }

}
