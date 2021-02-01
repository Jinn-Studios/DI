using JinnDev.Order.Models;

namespace JinnDev.OrderBadDI
{
    public static class AddressDTO
    {
        public static Shipping.Models.AddressModel ToShipping(this AddressModel source) => new Shipping.Models.AddressModel();
    }
}
