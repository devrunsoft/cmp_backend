using System;
using System.Collections.Generic;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class EditerviceAppointmentCommand : ServiceAppointmentInput, IRequest<CommandResponse<BaseServiceAppointment>>
    {
        public EditerviceAppointmentCommand(ServiceAppointmentInput input , long CompanyId, long ServiceId)
        {
            this.Id = input.Id;
            this.ServiceTypeId = input.ServiceTypeId;
            this.ProductPriceId = input.ProductPriceId;
            this.ProductId = input.ProductId;
            this.OperationalAddressId = input.OperationalAddressId;
            this.LocationCompanyIds = input.LocationCompanyIds;
            this.FrequencyType = input.FrequencyType;
            this.StartDate = input.StartDate;
            this.ServiceKind = input.ServiceKind;
            this.qty = input.qty;
            this.Subsidy = input.Subsidy;
            this.Amount = input.Amount;
            this.TotlaAmount = input.TotlaAmount;
            this.DayOfWeek = input.DayOfWeek;
            this.FromHour = input.FromHour;
            this.ToHour = input.ToHour;
            this.CompanyId = CompanyId;
            this.ServiceId = ServiceId;
        }

        public long CompanyId { get; set; }

        public long ServiceId { get; set; }
    }
}
