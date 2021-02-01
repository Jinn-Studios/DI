using FedEx;
using JinnDev.Order.Models;
using JinnDev.Shipping.Core;
using System.Collections.Generic;
using System.Linq;

namespace JinnDev.Shipping.FedEx
{
    public class FedExShipper : IShipStuff
    {
        private readonly IFedExService _fedExService;

        public FedExShipper(IFedExService fedExService) { _fedExService = fedExService; }

        public  List<ProductModel> CreateShipments(List<ProductModel> products, Models.AddressModel addr)
        {
            products.ForEach(x =>
            {
                var fedExReq = new FedExShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromAddress = new FedExAddress { StreetAddress = "1234 St", City = "OKC", State = "OK", Country = "US", AreaCode = 12345 },
                    ToAddress = new FedExAddress { StreetAddress = addr.Street, City = addr.City, State = addr.State, Country = "US", AreaCode = addr.ZIP },
                    Weight = x.Weight,
                    Height = x.Height,
                    Length = x.Length,
                    Width = x.Width,
                };
                x.ShippingGuid = _fedExService.CreateNewShipment(fedExReq);
            });

            return products;
        }

        public decimal CalculateStuff(Models.AddressModel toAddress, OrderModel newOrder)
        {
            var fedExRequests = newOrder.Products.Select(x =>
                new FedExShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromAddress = new FedExAddress { StreetAddress = "1234 St", City = "OKC", State = "OK", Country = "US", AreaCode = 12345 },
                    ToAddress = new FedExAddress { StreetAddress = toAddress.Street, City = toAddress.City, State = toAddress.State, Country = "US", AreaCode = toAddress.ZIP },
                    Weight = x.Weight,
                    Height = x.Height,
                    Length = x.Length,
                    Width = x.Width,
                }
            );

            return fedExRequests.Sum(x => _fedExService.CalculateShippingFees(x));
        }
    }
}