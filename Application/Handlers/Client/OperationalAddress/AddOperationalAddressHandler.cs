using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Model;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class AddOperationalAddressHandler : IRequestHandler<AddOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public AddOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            OperationalAddress lastOprAdd = (await _operationalAddressRepository.GetAsync(p => p.CompanyId == request.CompanyId)).FirstOrDefault();
            //if (lastOprAdd == null)
            //{
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
                return new Success<object>() { Data = result, Message = "OperationalAddres Added Successfully!" };
            //}
            //else
            //{
            //    return new NoAcess() { Message = "Operational Address Already Registred!" };
            //}
        }

    }
}
