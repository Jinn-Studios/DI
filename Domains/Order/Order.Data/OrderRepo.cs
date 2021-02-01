using JinnDev.Order.Data.Core;
using JinnDev.Order.Data.Entities;
using JinnDev.Order.Data.Enums;

namespace JinnDev.Order.Data
{
    public class OrderRepo : IOrderRepo
    {
        public OrderRepo(string connString) { }

        public int AddOrder(OrderEntity order) => 1;

        public OrderEntity GetOrderById(int orderId) => new OrderEntity { OrderId = orderId };

        public OrderStatus UpdateOrderStatus(OrderStatus status) => status;
    }
}