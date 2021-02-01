using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.Order.Core
{
    public interface IOrderService
    {
        // Also methods for GetByOrderId, CancelOrder, or whatever else...
        OrderModel CalculateOrder(int customerId, List<int> productIDs);
        bool CreateOrder(int orderId, PaymentInfoModel paymentInfo);
    }
}