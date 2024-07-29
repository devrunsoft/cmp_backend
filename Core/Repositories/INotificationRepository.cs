using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface INotificationRepository : IRepository<NotificationSystem, long>
    {
        Task<IReadOnlyList<NotificationSystem>> GetAllNotificationsListAsync();
        Task<IReadOnlyList<NotificationSystem>> GetAllNotificationsByShopIdAsync(int shopId);
        Task<NotificationSystem> GetNotificationByIdAsync(int id);
        Task<List<NotificationSystem>> GetAllUnSeenNotificationsListAsync(long PersonId);
    }
}
