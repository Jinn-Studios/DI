using JinnDev.Order.Data.Entities;
using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.OrderCI
{
    public static class DiscountDTO
    {
        public static DiscountModel CTM(this DiscountEntity source) => new DiscountModel();
        public static DiscountEntity CTE(this DiscountModel source) => new DiscountEntity();
        public static List<DiscountModel> CTMs(this List<DiscountEntity> source) => new List<DiscountModel>();
        public static List<DiscountEntity> CTEs(this List<DiscountModel> source) => new List<DiscountEntity>();
    }
}