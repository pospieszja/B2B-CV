using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Email;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;

        public NotificationService(INotificationRepository notificationRepository, IEmailService emailService)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
        }

        public async Task BroadcastAsync(ApplicationUser user, Dictionary<string, string> tags, string notificationName, string language)
        {
            var notification = await _notificationRepository.GetByLanguageAsync(notificationName, language);
            notification.ReplaceTagWithValue(tags);

            _emailService.Send(user.Email, notification.Subject, notification.Content);
        }

        public async Task BroadcastAsync(ApplicationUser user, string notificationName, string language)
        {
            var notification = await _notificationRepository.GetByLanguageAsync(notificationName, language);

            _emailService.Send(user.Email, notification.Subject, notification.Content);
        }
    }
}
