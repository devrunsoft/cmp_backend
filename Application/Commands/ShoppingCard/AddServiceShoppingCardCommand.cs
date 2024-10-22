using System;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{

    public class AddServiceShoppingCardCommand : ServiceAppointmentInput, IRequest<CommandResponse<ShoppingCard>>
    {
        public long CompanyId { get; set; }
        public long InvoiceId { get; set; }

        public string Name { get; set; }

        public string PriceName { get; set; }

        public string AddressName { get; set; }
    }
}

