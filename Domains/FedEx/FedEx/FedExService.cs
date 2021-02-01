using System;

namespace FedEx
{
    public class FedExService : IFedExService
    {
        public FedExService(string licenseNumber) { }
        public FedExShippingResult GetShippingDetails(Guid trackingNumber) => null;
        public Guid CreateNewShipment(FedExShippingRequest request) => Guid.Empty;
        public decimal CalculateShippingFees(FedExShippingRequest request) => 0;
    }
}