using JinnDev.Order.Data.Entities;
using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.OrderCI
{
    public static class ProductDTO
    {
        public static ProductModel CTM(this ProductEntity source) => new ProductModel();
        public static ProductEntity CTE(this ProductModel source) => new ProductEntity();
        public static List<ProductModel> CTMs(this List<ProductEntity> source) => new List<ProductModel>();
        public static List<ProductEntity> CTEs(this List<ProductModel> source) => new List<ProductEntity>();
    }
}
