using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Application.Responses.Invoice
{
    public class CreateInvoiceResponse
    {
        public ChatResponse ChatData { get; set; }
        public InboxStatusResponse InboxStatusData { get; set; } 
    }
}
