using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application.Handlers
{
    public class GetAllShoppingCardHandler : IRequestHandler<GetAllShoppingCardCommand, CommandResponse<List<ShoppingCard>>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;

        public GetAllShoppingCardHandler(IShoppingCardRepository shoppingCardRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;
        }

        public async Task<CommandResponse<List<ShoppingCard>>> Handle(GetAllShoppingCardCommand request, CancellationToken cancellationToken)
        {

            List<ShoppingCard> result = (await _shoppingCardRepository.GetAsync(p => p.CompanyId == request.CompanyId)).ToList();

            return new Success<List<ShoppingCard>>() { Data = result };
        }
    }
}

