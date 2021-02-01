namespace UPS
{
    public interface IUPSService
    {
        TrackedShipment CreateNewShipment(ShippingRequest request, string customerLicense);
        decimal CalculateShippingFees(ShippingRequest request, string customerLicense);
    }
}