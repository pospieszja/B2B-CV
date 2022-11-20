using HortimexB2B.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Infrastructure.ViewModels
{
    public class CatalogIndexViewModel
    {
        public IEnumerable<CatalogItem> CatalogItems { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
        public string Query { get; set; }
    }
}
