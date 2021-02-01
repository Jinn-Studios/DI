using System;

namespace FedEx
{
    public interface IFedExService
    {
        FedExShippingResult GetShippingDetails(Guid trackingNumber);
        Guid CreateNewShipment(FedExShippingRequest request);
        decimal CalculateShippingFees(FedExShippingRequest request);
    }
}