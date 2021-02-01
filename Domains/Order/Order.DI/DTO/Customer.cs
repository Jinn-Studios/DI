using JinnDev.Order.Models;
using JinnDev.Order.Data.Entities;

namespace JinnDev.OrderDI
{
    public static class CustomerDTO
    {
        public static CustomerModel CTM(this CustomerEntity source) => new CustomerModel { Address = new AddressModel() };
    }
}
