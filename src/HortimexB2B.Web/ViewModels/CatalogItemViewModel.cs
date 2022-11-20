using System.ComponentModel.DataAnnotations;

namespace HortimexB2B.Web.ViewModels
{
    public class CatalogItemViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double UnitPrice { get; set; }
        public string Unit { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Podaj wartość większą niż 1.")]
        public int Quantity { get; set; }
        public int StockId { get; set; }
    }
}
