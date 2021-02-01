using JinnDev.Order.Models;
using System.Collections.Generic;

namespace JinnDev.OrderCI
{
    public interface IShipStuff
    {
        decimal CalculateStuff(AddressModel toAddress, OrderModel newOrder);
        List<ProductModel> CreateShipments(List<ProductModel> products, AddressModel addr);
    }
}