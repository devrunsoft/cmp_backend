using System;
using System.Collections.Generic;
using System.Linq;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Service
{
	public class RegisterInvoiceService
	{
        IMediator _mediator;
        long rCompanyId;
        public  RegisterInvoiceService(IMediator mediator,long rCompanyId)
		{
            _mediator = mediator;
            this.rCompanyId = rCompanyId;
        }

        public async Task<List<CommandResponse<RequestEntity>>> call(long BillingInformationId)
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
                .GroupBy(s => new { s.OperationalAddressId })
                .Select(g => new
                {
                    OperationalAddressId = g.Key.OperationalAddressId,
                    Items = g.ToList()
                })
                .ToList();

            List<CommandResponse<RequestEntity>> invoices = new List<CommandResponse<RequestEntity>>();


            foreach (var i in groupedData)
            {
                var request = i.Items.Select((e) => new ServiceAppointmentInput()
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
                    DayOfWeek = e.DayOfWeek.IsNullOrEmpty()? Enum.GetValues(typeof(DayOfWeekEnum)).Cast<DayOfWeekEnum>().ToList() : e.DayOfWeek.Split(",").Select(x => (DayOfWeekEnum)Enum.Parse(typeof(DayOfWeekEnum), x.Trim())).ToList()
                    ,
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

              var invoice = await _mediator.Send(new CreateRequestCommand()
                {
                    CompanyId = rCompanyId,
                    //InvoiceCrmId = invoiceId.ToString(),
                    InvoiceNumber = invoiceId,
                    InvoiceId = invoiceId.ToString(),
                    Services = request,
                    Amount = 0,
                    OperationalAddressId = i.OperationalAddressId,
                    Address = i.Items.First().Address,
                    BillingInformationId = BillingInformationId

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

