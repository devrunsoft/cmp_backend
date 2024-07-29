using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using System.Linq;

namespace Bazaro.Application.Mapper
{
    public class ChatMappingProfile : Profile
    {

        private object getInvoice(Chat src)
        {
            if (src.TypeId == (int)Core.Enums.ChatType.Invoice && src.Invoices.Any())
            {
                var invoice = src.Invoices.First();

                var invoiceResponse =  new InvoiceResponse()
                {
                    Price = invoice.Price,
                    Discount = invoice.Discount == null ? null : new DiscountResponse()
                    {
                        Id = invoice.Discount.Id,
                        IsEnable = invoice.Discount.IsEnable,
                        Percent = invoice.Discount.Percent,
                        Price = invoice.Discount.Price,
                        Description = invoice.Discount.Description,
                        //Logo = invoice.Discount.Logo,
                    },
                    Delivery = new DeliveryResponse()
                    {
                        Id = invoice.Delivery.Id,
                        Title = invoice.Delivery.Title,
                        Name = invoice.Delivery.Name,
                        Price = invoice.Delivery.Price,
                        CreatedAt = invoice.Delivery.CreatedAt,
                    },
                };

                return invoiceResponse;
            }

            return null;
        }

        public ChatMappingProfile()
        {
            CreateMap<Chat, ChatResponse>()
                .ForMember(x => x.Invoice, opt => opt.MapFrom(src => getInvoice(src)))
                .ReverseMap();
        }

    }
}
