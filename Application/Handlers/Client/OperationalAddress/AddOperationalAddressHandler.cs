using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddOperationalAddressHandler : IRequestHandler<AddOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly IMediator _mediator;
        public AddOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository, IMediator _mediator)
        {
            _operationalAddressRepository = operationalAddressRepository;
            this._mediator = _mediator;
        }

        public async Task<CommandResponse<object>> Handle(AddOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            OperationalAddress lastOprAdd = (await _operationalAddressRepository.GetAsync(p => p.CompanyId == request.CompanyId)).FirstOrDefault();

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

              return new Success<object>() { Data = result, Message = "OperationalAddres Added Successfully!" };

        }

    }
}
