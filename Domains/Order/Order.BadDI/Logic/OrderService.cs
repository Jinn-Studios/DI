using JinnDev.Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JinnDev.OrderBadDI
{
    public partial class OrderService
    {
        public static decimal CalculateNetTotal(List<ProductModel> products, List<DiscountModel> discounts)
            => products.Sum(x => x.Total / (discounts.FirstOrDefault(y => y.ProductID == x.Id)?.Discount ?? 1));

        public static OrderModel CalculateOrder(List<ProductModel> products, List<DiscountModel> discounts, int customerId, Func<OrderModel, decimal> calculateStuff)
        {
            var newOrder = new OrderModel
            {
                CustomerID = customerId,
                Products = products,
                NetTotal = CalculateNetTotal(products, discounts)
            };

            newOrder.Shipping = calculateStuff(newOrder);
            return newOrder;
        }
    }
}