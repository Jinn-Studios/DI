using JinnDev.Order.Enums;
using System;
using System.Collections.Generic;

namespace JinnDev.Order.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public decimal Total { get => NetTotal + Shipping; }
        public decimal Shipping { get; set; }
        public OrderStatus Status { get; set; }
        public decimal NetTotal { get; set; }
        public int CustomerID { get; set; }

        public List<ProductModel> Products { get; set; }
        public Guid ShippingGUID { get; internal set; }
    }
}