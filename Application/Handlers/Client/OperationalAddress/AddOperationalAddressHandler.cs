using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddOperationalAddressHandler : IRequestHandler<AddOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ICapacityRepository _capacityRepository;
        private readonly IMediator _mediator;
        public AddOperationalAddressHandler(
            IOperationalAddressRepository operationalAddressRepository,
            ICapacityRepository capacityRepository,
            IMediator mediator)
        {
            _operationalAddressRepository = operationalAddressRepository;
            _capacityRepository = capacityRepository;
            _mediator = mediator;
        }

        public async Task<CommandResponse<object>> Handle(AddOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = new Core.Entities.OperationalAddress()
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

    }
}
