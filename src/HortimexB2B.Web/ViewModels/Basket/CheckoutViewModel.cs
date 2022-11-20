using HortimexB2B.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HortimexB2B.Web.ViewModels.Basket
{
    public class CheckoutViewModel
    {
        public BasketViewModel Basket { get; set; }
        public IEnumerable<ShippingAddressViewModel> Addresses { get; set; }

        public List<SelectListItem> AddressSelectList { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Uwagi")]
        [MaxLength(80)]
        public string Comments { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nr zamówienia klienta")]
        [MaxLength(100)]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Adres dostawy")]
        public int SelectedAddress { get; set; }

        [Display(Name = "Planowana data dostawy")]
        public DateTime DesiredDeliveryDate { get; set; }
        public DateTime MinDesiredDeliveryDate { get; set; }
    }
}
