using Bazaro.Core.Entities;
using BazaroApp.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Core.Repositories
{
    public interface IAssistantRepository : IRepository<ShopUser>
    {
        Task SetAssistantEnable(long shopUserId);
    }
}
