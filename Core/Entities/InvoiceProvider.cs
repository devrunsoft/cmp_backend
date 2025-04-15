//using System;
//namespace CMPNatural.Core.Entities
//{
//	public class InvoiceProvider : Invoice
//    {
//        public long InvoiceIdentity { get; set; }
//    }

//    public static class InvoiceMapper
//    {
//        public static InvoiceProvider ToInvoiceProvider(this Invoice invoice)
//        {
//            if (invoice == null) return null;

//            return new InvoiceProvider
//            {
//                Id = invoice.Id,
//                CompanyId = invoice.CompanyId,
//                ProviderId = invoice.ProviderId,
//                InvoiceCrmId = invoice.InvoiceCrmId,
//                Status = invoice.Status,
//                Link = invoice.Link,
//                Amount = invoice.Amount,
//                InvoiceId = invoice.InvoiceId,
//                SendDate = invoice.SendDate,
//                CreatedAt = invoice.CreatedAt,
//                OperationalAddressId = invoice.OperationalAddressId,
//                Address = invoice.Address,
//                Company = invoice.Company,
//                Provider = invoice.Provider,
//                ContractId = invoice.ContractId,
//                InvoiceProduct = invoice.InvoiceProduct,
//                BaseServiceAppointment = invoice.BaseServiceAppointment
//            };
//        }
//    }

//}

