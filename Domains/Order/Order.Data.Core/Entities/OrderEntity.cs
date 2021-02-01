using JinnDev.Order.Data.Enums;
using System;
using System.Collections.Generic;

namespace JinnDev.Order.Data.Entities
{
    public class OrderEntity
    {
        public int OrderId { get; set; }
        public decimal Total { get => NetTotal + Shipping; }
        public decimal Shipping { get; set; }
        public OrderStatus Status { get; set; }
        public decimal NetTotal { get; set; }
        public int CustomerID { get; set; }

        public List<ProductEntity> Products { get; set; }
        public Guid ShippingGUID { get; internal set; }
    }
}