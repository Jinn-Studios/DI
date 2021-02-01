using System.Collections.Generic;

namespace JinnDev.Order.Data.Entities
{
    public class CustomerEntity
    {
        public int Id { get; set; }
        public AddressEntity Address { get; set; }
        public List<DiscountEntity> Discounts { get; internal set; }
    }
}