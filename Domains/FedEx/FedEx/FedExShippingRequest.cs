namespace FedEx
{
    public class FedExShippingRequest
    {
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal DeclaredValue { get; set; }
        public FedExAddress FromAddress { get; set; }
        public FedExAddress ToAddress { get; set; }
    }
}