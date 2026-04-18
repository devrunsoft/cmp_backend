using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class CreateDriverForgotPasswordLinkHandler : IRequestHandler<CreateDriverForgotPasswordLinkCommand, CommandResponse<Driver>>
    {
        private readonly IDriverRepository _driverRepository;

        public CreateDriverForgotPasswordLinkHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<CommandResponse<Driver>> Handle(CreateDriverForgotPasswordLinkCommand request, CancellationToken cancellationToken)
        {
            var driver = (await _driverRepository.GetAsync(x => x.Email == request.Email)).FirstOrDefault();

            if (driver == null)
            {
                return new NoAcess<Driver>() { };
            }

            driver.ActivationLink = Guid.NewGuid();
            await _driverRepository.UpdateAsync(driver);

            return new Success<Driver>() { Data = driver };
        }
    }
}
