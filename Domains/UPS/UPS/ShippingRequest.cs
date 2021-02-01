namespace UPS
{
    public class ShippingRequest
    {
        public decimal Weight { get; set; }
        public decimal CentimetersCubed { get; set; }
        public decimal DeclaredValue { get; set; }
        public int FromZIP { get; set; }
        public int ToZIP { get; set; }
    }
}