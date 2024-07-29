using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;
using System.Linq.Expressions;

namespace Bazaro.Core.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice, long>
    {
        Task<Invoice> AddInvoiceAsync(long InboxId, Invoice entity);

        Task<List<Invoice>> InvoiceWithChatAsync(long[] inboxIds);
    }
}
