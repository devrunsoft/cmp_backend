using Bazaro.Application.Commands;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class GetShopUsersPhoneHandler : IRequestHandler<GetShopUsersPhoneQuery, IEnumerable<string>>
    {
        private readonly IShopUserRepository _shopUserRepository;

        public GetShopUsersPhoneHandler(IShopUserRepository shopUserRepository, IMediator mediator)
        {
            _shopUserRepository = shopUserRepository; 
        }
        public async Task<IEnumerable<string>> Handle(GetShopUsersPhoneQuery request, CancellationToken cancellationToken)
        {
            var users = await _shopUserRepository.GetShopUsers(request.ShopId); 
            return users.Select(p => /*p.Item1 +*/ p.Item2.Person.Mobile);
        }
    }
}
