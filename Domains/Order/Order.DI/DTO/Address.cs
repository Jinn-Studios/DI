using JinnDev.Order.Models;

namespace JinnDev.OrderDI
{
    public static class AddressDTO
    {
        public static Shipping.Models.AddressModel ToShipping(this AddressModel source) => new Shipping.Models.AddressModel();
    }
}
