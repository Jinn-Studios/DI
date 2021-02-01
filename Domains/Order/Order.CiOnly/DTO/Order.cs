using JinnDev.Order.Data.Entities;
using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.OrderCI
{
    public static class OrderDTO
    {
        public static OrderEntity CTE(this OrderModel source) => new OrderEntity { Products = new List<ProductEntity>() };
        public static OrderModel CTM(this OrderEntity source) => new OrderModel { Products = new List<ProductModel>() };
    }
}