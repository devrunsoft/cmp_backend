using System;
using System.Collections.Generic;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Service
{
	public class RegisterInvoiceService
	{
        IMediator _mediator;
        long rCompanyId;
        public RegisterInvoiceService(IMediator mediator,long rCompanyId)
		{
            _mediator = mediator;
            this.rCompanyId = rCompanyId;
        }

		public async Task<List<CommandResponse<Invoice>>> call()
		{
            var resultShopping = (await _mediator.Send(new GetAllShoppingCardCommand()
            {
                CompanyId = rCompanyId,
            })).Data;

            var company = (await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })).Data;

            var groupedData = resultShopping
             .GroupBy(s => s.OperationalAddressId)
                 .Select(g => new
                 {
                     oprId = g.Key,
                     item = g
                 })
              .ToList();

            List<CommandResponse<Invoice>> invoices = new List<CommandResponse<Invoice>>();

            foreach (var i in groupedData)
            {

                var request = i.item.Select((e) => new ServiceAppointmentInput()
                {
                    FrequencyType = e.FrequencyType,
                    OperationalAddressId = e.OperationalAddressId,
                    ServiceKind = (ServiceKind)e.ServiceKind,
                    //ServicePriceId=e.ServicePriceCrmId,
                    ServiceTypeId = (ServiceType)e.ServiceId,
                    StartDate = e.StartDate,
                    //ServiceCrmId = e.ServiceCrmId,
                    ProductPriceId = e.ProductPriceId.Value,
                    ProductId = e.ProductId.Value,
                    LocationCompanyIds = e.LocationCompanyIds.IsNullOrEmpty() ? new List<long>() : e.LocationCompanyIds.Split(",").Select((e) => long.Parse(e)).ToList(),
                    qty = e.Qty,
                    DayOfWeek = e.DayOfWeek.Split(",").Select(x => (DayOfWeekEnum)Enum.Parse(typeof(DayOfWeekEnum), x.Trim())).ToList(),
                    FromHour = e.FromHour,
                    ToHour = e.ToHour
                }).ToList();


                var invoiceId = Guid.NewGuid();

                var resultInvoice = await _mediator.Send(new AddInvoiceSourceCommand()
                {
                    CompanyId = rCompanyId,
                    InvoiceId = invoiceId.ToString(),
                });

                if (!resultInvoice.IsSucces())
                {
                    return invoices;
                }

              var invoice = await _mediator.Send(new CreateInvoiceCommand()
                {
                    CompanyId = rCompanyId,
                    InvoiceCrmId = invoiceId.ToString(),
                    InvoiceNumber = invoiceId,
                    InvoiceId = invoiceId.ToString(),
                    Services = request,
                    Amount = 0,
                    OperationalAddressId = i.oprId,
                    Address = i.item.First().Address

                });
                invoices.Add(invoice);

            }

            var result = await _mediator.Send(new DeleteAllShoppingCardCommand()
            {
                CompanyId = rCompanyId,
            });

            return invoices;
        }

	}
}

