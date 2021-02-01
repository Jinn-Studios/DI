using JinnDev.Order.Data.Entities;
using JinnDev.Order.Models;

namespace JinnDev.OrderCI
{
    public static class CustomerDTO
    {
        public static CustomerModel CTM(this CustomerEntity source) => new CustomerModel { Address = new AddressModel() };
    }
}