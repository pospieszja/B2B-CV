using System;
using System.Collections.Generic;
using System.Linq;

namespace HortimexB2B.Web.ViewModels.Basket
{
    public class BasketViewModel
    {
        private int _minimalValueOfBasket = 1000;

        public Guid UserId { get; set; }
        public Guid BasketId { get; set; }
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();

        public bool CanBeOrdered
        {
            get
            {
                return TotalValue >= _minimalValueOfBasket ? true : false;
            }
        }

        public double TotalValue
        {
            get
            {
                return Items.Sum(i => i.UnitPrice * i.ConvertedQuantity);
            }
        }

        public double TotalValueWithVat
        {
            get
            {
                return Items.Sum(i => Math.Round((i.UnitPrice * i.ConvertedQuantity) * (1 + i.Vat / 100), 2));
            }
        }
    }
}
