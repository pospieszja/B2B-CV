using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortimexB2B.Web.ViewModels.User
{
    public class UserInfoViewModel
    {
        public string Email { get; set; }
        public string GraffitiID { get; set; }
        public string NIP { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; } = new Address();
        public double CreditLimit { get; set; }
        public int PaymentTerms { get; set; }
        public SalesManager SalesManager { get; set; } = new SalesManager();
    }

    public class Address
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }

    public class SalesManager
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
