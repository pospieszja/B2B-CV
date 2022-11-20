using HortimexB2B.Core.Domain;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> GetByLanguageAsync(string name, string language);
    }
}
