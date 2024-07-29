using Bazaro.Application.Commands;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Validator;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetShopOwnerHandler : IRequestHandler<SetShopOwnerCommand, CommandResponse>
    {
        private readonly IShopRepository _shopRepository;

        public SetShopOwnerHandler(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task<CommandResponse> Handle(SetShopOwnerCommand request, CancellationToken cancellationToken)
        {
            var shop = await _shopRepository.GetByIdAsync(request.ShopId);
            shop.OwnerId = request.PersonId;

            await _shopRepository.UpdateAsync(shop);

            return new Success();
        }
    }
}
