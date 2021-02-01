namespace UPS
{
    public class UPSService : IUPSService
    {
        public UPSService() { }
        public TrackedShipment CreateNewShipment(ShippingRequest request, string customerLicense) => null;
        public decimal CalculateShippingFees(ShippingRequest request, string customerLicense) => 0;
    }
}