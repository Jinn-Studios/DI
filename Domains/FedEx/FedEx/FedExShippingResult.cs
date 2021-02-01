using System;
using System.Collections.Generic;

namespace FedEx
{
    public class FedExShippingResult : FedExShippingRequest
    {
        public Guid TrackingNumber { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Tax { get; set; }
        public decimal AirFare { get; set; }
        public DateTime ArrivalDate { get; set; }
        public List<ShippingStatus> Statuses { get; set; }
    }
}