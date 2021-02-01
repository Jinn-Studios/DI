using System;

namespace UPS
{
    public class TrackedShipment
    {
        public long TrackingNumber { get; set; }
        public decimal Cost { get; set; }
        public DateTime ArrivalDate { get; set; }
        public ShippingStatus Status { get; set; }
    }
}