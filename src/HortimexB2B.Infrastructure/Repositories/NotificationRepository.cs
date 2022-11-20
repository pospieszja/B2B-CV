using HortimexB2B.Core.Domain;
using HortimexB2B.Infrastructure.EF;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly B2BContext _context;

        public NotificationRepository(B2BContext context)
        {
            _context = context;
        }

        public async Task<Notification> GetByLanguageAsync(string name, string language)
        {
            return await _context.Notifications.SingleOrDefaultAsync(x => x.Name == name && x.Language == language);
        }
    }
}
