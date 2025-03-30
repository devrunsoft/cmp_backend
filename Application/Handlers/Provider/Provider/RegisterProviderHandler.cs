using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMPNatural.Application
{
    public class RegisterProviderHandler : IRequestHandler<RegisterProviderCommand, CommandResponse<Provider>>
    {
        private readonly IProviderReposiotry _repository;

        public RegisterProviderHandler(IProviderReposiotry repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Provider>> Handle(RegisterProviderCommand request, CancellationToken cancellationToken)
        {
            var emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return new NoAcess<Provider>() { Message = "The email format is invalid. Please provide a valid email address." };
            }

            var isExistEmail = (await _repository.GetAsync(x => x.Email == request.Email)).Any();
            if (isExistEmail)
            {
                return new NoAcess<Provider>() { Message = "This email is already registered. Please use a different email." };
            }

            var entity = new Provider()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                AreaLocation = 0,
                Lat= 0,
                Long = 0,
                Status = ProviderStatus.Pending,
                RegistrationStatus = ProviderRegistrationStatus.Basic_Information
            };

            var result = await _repository.AddAsync(entity);

            return new Success<Provider>() { Data = result };
        }
    }
}

