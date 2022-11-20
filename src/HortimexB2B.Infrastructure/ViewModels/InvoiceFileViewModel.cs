using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HortimexB2B.Infrastructure.ViewModels
{
    public class InvoiceFileViewModel
    {
        public byte[] RawFile { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
    }
}
