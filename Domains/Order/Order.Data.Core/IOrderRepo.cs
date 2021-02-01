using JinnDev.Order.Data.Entities;
using JinnDev.Order.Data.Enums;

namespace JinnDev.Order.Data.Core
{
    public interface IOrderRepo
    {
        int AddOrder(OrderEntity order);
        OrderEntity GetOrderById(int orderId);
        OrderStatus UpdateOrderStatus(OrderStatus status);
    }
}