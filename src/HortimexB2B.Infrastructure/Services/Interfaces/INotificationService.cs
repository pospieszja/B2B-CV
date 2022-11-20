using HortimexB2B.Infrastructure.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task BroadcastAsync(ApplicationUser user, Dictionary<string, string> tags, string notificationName, string language);
        Task BroadcastAsync(ApplicationUser user, string notificationName, string language);
    }
}
