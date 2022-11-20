using HortimexB2B.Infrastructure.ViewModels;
using System.Threading.Tasks;

namespace HortimexB2B.Infrastructure.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileViewModel> GetFile(string itemId, int documentType);
        Task<InvoiceFileViewModel> GetFile(int orderId, int orderYear, int companyId);
        Task<BlobFileViewModel> GetFile(int documentId);
    }
}
