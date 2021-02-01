using JinnDev.Order.Data.Entities;

namespace JinnDev.Order.Data.Core
{
    public interface ICustomerRepo
    {
        CustomerEntity GetCustomerById(int customerId);
    }
}