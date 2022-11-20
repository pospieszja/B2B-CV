using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using HortimexB2B.Infrastructure.Settings;
using HortimexB2B.Infrastructure.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebDav;

namespace HortimexB2B.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly OwnCloudSettings _settings;
        private readonly IOrderRepository _orderRepository;

        public FileService(IOptions<OwnCloudSettings> settings, IOrderRepository orderRepository)
        {
            _settings = settings.Value;
            _orderRepository = orderRepository;
        }

        public async Task<InvoiceFileViewModel> GetFile(int orderId, int orderYear, int companyId)
        {
            var vm = await _orderRepository.GetInvoiceByOrder(orderId, orderYear, companyId);

            return vm;
        }

        public async Task<BlobFileViewModel> GetFile(int documentId)
        {
            var vm = await _orderRepository.GetFile(documentId);

            return vm;
        }

        public async Task<FileViewModel> GetFile(string itemId, int documentType)
        {
            IWebDavClient _client = new WebDavClient();

            string path = "";

            switch (documentType)
            {
                case 1:
                    path = "MSDS";
                    break;
                case 2:
                    path = "Specyfikacje";
                    break;
                default:
                    break;
            }

            var vm = new FileViewModel();

            string fileUri;
            WebDavStreamResponse response;

            var propfindParams = new PropfindParameters
            {
                Headers = new[] { new KeyValuePair<string, string>("Depth", "infinity") }
            };

            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri(_settings.Uri),
                Credentials = new NetworkCredential(_settings.User, _settings.Password)
            };

            _client = new WebDavClient(clientParams);

            var result = await _client.Propfind(_settings.Uri + path, propfindParams);
            if (result.IsSuccessful)
            {
                foreach (var res in result.Resources)
                {
                    if (res.Uri.Contains(itemId.Replace("-", "_")) && res.Uri.Contains("pdf") && !res.Uri.Contains("ARCHIWUM"))
                    {
                        fileUri = res.Uri;
                        response = await _client.GetRawFile(fileUri);
                        vm.RawFile = response.Stream;
                        vm.ContentType = res.ContentType;
                        vm.Name = itemId + "_" + path + ".pdf";
                    }
                }
            }
            return vm;
        }
    }
}
