using System.Collections.Generic;

namespace JinnDev.Order.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public AddressModel Address { get; set; }
        public List<DiscountModel> Discounts { get; internal set; }
    }
}