using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetAssistantStatusHandler : IRequestHandler<SetAssistantStatusCommand, CommandResponse>
    {
        private readonly IShopUserRepository _shopUserRepository;

        public SetAssistantStatusHandler(IShopUserRepository shopUserRepository)
        {
            _shopUserRepository = shopUserRepository;
        }

        public async Task<CommandResponse> Handle(SetAssistantStatusCommand request, CancellationToken cancellationToken)
        {
            var assistant = await _shopUserRepository.GetByIdAsync(request.ShopUserId);
            if (assistant == null)
                return new ResponseNotFound();

            assistant.IsEnable = request.IsEnable;
            await _shopUserRepository.UpdateAsync(assistant);

            return new Success() { Data = assistant };
        }
    }
}
