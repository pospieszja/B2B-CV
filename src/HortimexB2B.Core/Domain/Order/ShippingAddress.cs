namespace HortimexB2B.Core.Domain.Order
{
    public class ShippingAddress
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string Post { get; private set; }
    }
}