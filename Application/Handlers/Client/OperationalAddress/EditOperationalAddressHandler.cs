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
using CMPNatural.Application.Commands.OperationalAddress;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class EditOperationalAddressHandler : IRequestHandler<EditOperationalAddressCommand, CommandResponse<object>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;

        public EditOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
        }

        public async Task<CommandResponse<object>> Handle(EditOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            var entity = (await _operationalAddressRepository.GetAsync(x=>x.Id ==request.Id, query => query.Include(x => x.LocationDateTimes))).FirstOrDefault();

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to edit!" };
            }
            List<LocationDateTime> LocationDateTimes = new List<LocationDateTime>();

            foreach (var x in request.LocationDateTimeInputs)
            {
                if (x.Id == null)
                {
                    // Add new
                    LocationDateTimes.Add(new LocationDateTime
                    {
                        CompanyId = request.CompanyId,
                        DayName = x.DayName,
                        FromTime = x.FromTime,
                        ToTime = x.ToTime,
                        OperationalAddressId = request.Id
                    });
                }
                else
                {
                    // Update existing
                    var existing = entity.LocationDateTimes.FirstOrDefault(ldt => ldt.Id == x.Id.Value);
                    if (existing != null)
                    {
                        existing.DayName = x.DayName;
                        existing.FromTime = x.FromTime;
                        existing.ToTime = x.ToTime;
                    }
                    LocationDateTimes.Add(existing);
                }
            }


            entity.Address = request.Address;
            entity.BusinessId = request.BusinessId;
            entity.CompanyId = request.CompanyId;
            entity.County = request.County;
            entity.CrossStreet = request.CrossStreet;
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.LocationPhone = request.LocationPhone;
            entity.Lat = request.Lat;
            entity.Long = request.Long;
            entity.Name = request.Name;
            entity.LocationDateTimes = LocationDateTimes;


            await _operationalAddressRepository.UpdateAsync(entity);


            return new CommandResponse<object>() { Success = true, Data = entity, Message = "OperationalAddres updated successfully!" };
        }

    }
}
