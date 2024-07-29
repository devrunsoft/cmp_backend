using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class RegisterShopUserHandler : IRequestHandler<RegisterShopUserCommand, RegisterShopUserResponse>
    {
        private readonly IShopUserRepository _shopUserRepository;

        public RegisterShopUserHandler(IShopUserRepository shopUserRepository)
        {
            _shopUserRepository = shopUserRepository;
        }

        public async Task<RegisterShopUserResponse> Handle(RegisterShopUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _shopUserRepository.GetShopUserByPersonId(request.PersonId, false);

            if (user == null)
            {
                user = new ShopUser()
                {
                    ShopId = request.ShopId,
                    PersonId = request.PersonId,
                    IsActive = false,
                    CreatedAt = DateTime.Now,
                    IsEnable = true,
                    IsDeleted = false,
                    RoleId = (int)ShopUserRole.Manager
                };

                await _shopUserRepository.AddAsync(user);
            }

            return new RegisterShopUserResponse() { Id = user.Id };
        }
    }
}
