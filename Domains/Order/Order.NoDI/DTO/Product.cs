using JinnDev.Order.Data.Entities;
using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.OrderNoDI
{
    public static class ProductDTO
    {
        public static List<ProductModel> CTMs(this List<ProductEntity> source) => new List<ProductModel>();
    }
}
