using System;

namespace JinnDev.Order.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public Guid ShippingGuid { get; set; }
    }
}