using JinnDev.Order.Data.Core;
using JinnDev.Order.Data.Entities;

namespace JinnDev.Order.Data
{
    public class CustomerRepo : ICustomerRepo
    {
        public CustomerRepo(string connString) { }

        public CustomerEntity GetCustomerById(int customerId) => new CustomerEntity { Id = customerId };
    }
}