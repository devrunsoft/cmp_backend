using System;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddServiceAppointmentCommand : ServiceAppointmentInput, IRequest<CommandResponse<BaseServiceAppointment>>
    {
        public AddServiceAppointmentCommand(ServiceAppointmentInput input, long CompanyId, long InvoiceId)
        {
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
            this.InvoiceId = InvoiceId;
        }

        public long CompanyId { get; set; }
        public long InvoiceId { get; set; }
    }
}

