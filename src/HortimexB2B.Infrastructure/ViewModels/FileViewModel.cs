using System.IO;

namespace HortimexB2B.Infrastructure.ViewModels
{
    public class FileViewModel
    {
        public Stream RawFile { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }
}
