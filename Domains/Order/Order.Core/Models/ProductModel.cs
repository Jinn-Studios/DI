using System;

namespace JinnDev.Order.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public Guid ShippingGuid { get; set; }
        public string ShippingIdentifier { get; set; }
    }
}