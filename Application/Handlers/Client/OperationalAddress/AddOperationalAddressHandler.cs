using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Enums;
using Microsoft.Extensions.Options;
using CMPNatural.Core.Models;
using ScoutDirect.Core.Repositories;
using Stripe.Forwarding;

namespace CMPNatural.Application
{
    public class AddOperationalAddressHandler : IRequestHandler<AddOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ICapacityRepository _capacityRepository;
        private readonly IMediator _mediator;
        private readonly AppConfig Config;
        public AddOperationalAddressHandler(
            IOperationalAddressRepository operationalAddressRepository,
            ICapacityRepository capacityRepository,
            IMediator mediator,
            IOptions<AppConfig> Config,
            ICompanyRepository companyRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
            _capacityRepository = capacityRepository;
            _mediator = mediator;
            this.Config = Config.Value;
            this.companyRepository = companyRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            var company = (await companyRepository.GetByIdAsync(request.CompanyId));

            var username = Config.AddressId ? await GenerateId(company) : null;
            var entity = new OperationalAddress()
                {
                    Address = request.Address,
                    BusinessId = request.BusinessId,
                    CompanyId = request.CompanyId,
                    County = request.County,
                    CrossStreet = request.CrossStreet,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    LocationPhone = request.LocationPhone,
                    Lat = request.Lat,
                    Long = request.Long,
                    Name = request.Name,
                    Username = username,
                    //Username = request.Username,
                    Password = request.Password,
                      LocationDateTimes = request.LocationDateTimeInputs.Select(x => new LocationDateTime()
                    {
                        CompanyId = request.CompanyId,
                        DayName = x.DayName,
                        FromTime = x.FromTime,
                        ToTime = x.ToTime

                    }).ToList()
                };

                var result = await _operationalAddressRepository.AddAsync(entity);

                var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.CompanyId,
                    OperationalAddressId = result.Id })).Data;

                var defaultCapacity = (await _capacityRepository.GetAsync(x => x.Enable)).FirstOrDefault();
                if (defaultCapacity != null && request.Lat != 0 && request.Long != 0)
                {
                    await _mediator.Send(new AddLocationCompanyCommand()
                    {
                        CompanyId = request.CompanyId,
                        OperationalAddressId = result.Id,
                        Name = request.Name,
                        Address = request.Address,
                        Lat = request.Lat,
                        Long = request.Long,
                        Comment = string.Empty,
                        PrimaryFirstName = request.FirstName,
                        PrimaryLastName = request.LastName,
                        PrimaryPhonNumber = request.LocationPhone,
                        CapacityId = defaultCapacity.Id,
                        Type = LocationType.Other
                    });
                }

              return new Success<object>() { Data = result, Message = "OperationalAddres Added Successfully!" };

        }

        private async Task<string> GenerateId(Company company)
        {
            var prefix = GetPrefix(company.CompanyName);

            var existingNumbers = (await _operationalAddressRepository
                .GetAsync(x => x.Username.StartsWith(prefix)))
                .Select(x=>x.Username)
                .ToList();

            var maxNumber = 0;

            foreach (var accountNumber in existingNumbers)
            {
                if (accountNumber.Length <= prefix.Length)
                    continue;

                var numberPart = accountNumber.Substring(prefix.Length);

                if (int.TryParse(numberPart, out var parsedNumber) && parsedNumber > maxNumber)
                {
                    maxNumber = parsedNumber;
                }
            }

            var nextNumber = maxNumber + 1;

            return $"{prefix}{nextNumber:D3}";
        }

        private string GetPrefix(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return "ACC";

            var cleaned = new string(companyName
                .Where(char.IsLetter)
                .ToArray());

            if (string.IsNullOrWhiteSpace(cleaned))
                return "ACC";

            var prefix = cleaned.Length >= 3
                ? cleaned.Substring(0, 3)
                : cleaned;

            return char.ToUpper(prefix[0]) + prefix.Substring(1).ToLower();
        }

    }
}
