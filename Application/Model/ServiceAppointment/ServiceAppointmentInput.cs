using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model.ServiceAppointment
{
    public partial class ServiceAppointmentInput
    {
        public long? Id { get; set; }

        public long? tempId { get; set; }

        public ServiceType ServiceTypeId { get; set; } = ServiceType.Cooking_Oil_Collection;

        public long ProductPriceId { get; set; }

        public long ProductId { get; set; }

        public long OperationalAddressId { get; set; }

        public List<long> LocationCompanyIds { get; set; }

        public string FrequencyType { get; set; } = null;

        public DateTime? StartDate { get; set; } = null;

        public ServiceKind ServiceKind { get; set; } = ServiceKind.Custom;

        public int qty { get; set; } = 1;

        public int? FactQty { get; set; }

        public double Subsidy { get; set; } = 0;

        public double Amount { get; set; } = 0;

        public double TotlaAmount { get; set; } = 0;

        public List<DayOfWeekEnum> DayOfWeek { get; set; }

        public int FromHour { get; set; } = 480; //8 AM

        public int ToHour { get; set; } = 1080; //6 PM

        public OilQualityEnum? OilQuality { get; set; }

        public List<ServiceAppointmentLocationInput> ServiceAppointmentLocations { get; set; }


    }

    public partial class ServiceAppointmentLocationInput
    {
        public long LocationCompanyId { get; set; }

        public int? FactQty { get; set; }

        public OilQualityEnum? OilQuality { get; set; }
    }


    public static class ServiceAppointmentMapper
    {
        public static BaseServiceAppointment ToEntity(this ServiceAppointmentInput input, BaseServiceAppointment model, long companyId, long invoiceId)
        {
            if (input == null) return null;

            model.Id = input.Id ?? 0;
            model.ServiceTypeId = (long)input.ServiceTypeId;
            model.ProductPriceId = input.ProductPriceId;
            model.ProductId = input.ProductId;
            model.OperationalAddressId = input.OperationalAddressId;
            model.CompanyId = companyId;
            model.InvoiceId = invoiceId;
            model.Qty = input.qty;
            model.Subsidy = input.Subsidy;
            model.Amount = input.Amount;
            model.TotalAmount = (decimal)input.TotlaAmount;
            model.StartDate = input.StartDate ?? DateTime.UtcNow;
            model.DayOfWeek = input.DayOfWeek != null ? string.Join(",", input.DayOfWeek.Select(d => d.ToString())) : "";
            model.FromHour = input.FromHour;

            return model;
        }
    }

    //public partial class ProviderServiceAppointmentInput : ServiceAppointmentInput
    //{
    //    public long Id { get; set; }
    //}

    public partial class ServiceAppointmentInput
    {

       public double calculateAmount()
        {
            var amount = (this.qty * this.Amount) - this.Subsidy;
            return amount;
        }

    }

}