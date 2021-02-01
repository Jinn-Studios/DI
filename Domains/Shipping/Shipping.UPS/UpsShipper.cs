using UPS;
using JinnDev.Order.Models;
using JinnDev.Shipping.Core;
using System.Collections.Generic;
using System.Linq;

namespace JinnDev.Shipping.UPS
{
    public class UpsShipper : IShipStuff
    {
        private readonly IUPSService _upsService;
        private readonly string _upsLicense;

        public UpsShipper(IUPSService upsService, string upsLicense)
        {
            _upsService = upsService;
            _upsLicense = upsLicense;
        }

        public List<ProductModel> CreateShipments(List<ProductModel> products, Models.AddressModel addr)
        {
            products.ForEach(x =>
            {
                var fedExReq = new ShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromZIP = 12345,
                    ToZIP = addr.ZIP,
                    Weight = x.Weight,
                    CentimetersCubed = x.Height * x.Length * x.Width
                };
                //x.ShippingGuid = _upsService.CreateNewShipment(fedExReq, _upsLicense).TrackingNumber;
            });

            return products;
        }

        public decimal CalculateStuff(Models.AddressModel toAddress, OrderModel newOrder)
        {
            var fedExRequests = newOrder.Products.Select(x =>
                new ShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromZIP = 12345,
                    ToZIP = toAddress.ZIP,
                    Weight = x.Weight,
                    CentimetersCubed = x.Height * x.Length * x.Width
                }
            );

            return fedExRequests.Sum(x => _upsService.CalculateShippingFees(x, _upsLicense));
        }
    }
}