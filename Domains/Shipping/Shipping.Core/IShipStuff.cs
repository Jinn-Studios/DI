using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.Shipping.Core
{
    public interface IShipStuff
    {
        List<ProductModel> CreateShipments(List<ProductModel> products, Models.AddressModel addr);
        decimal CalculateStuff(Models.AddressModel toAddress, OrderModel newOrder);
    }
}