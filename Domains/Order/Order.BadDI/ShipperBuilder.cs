using JinnDev.Shipping.Core;

namespace JinnDev.OrderBadDI
{
    public class ShipperBuilder : IBuildShips
    {
        readonly IShipStuff _fedExShipper;
        readonly IShipStuff _upsShipper;

        public ShipperBuilder(IShipStuff fedExShipper, IShipStuff upsShipper)
        {
            _fedExShipper = fedExShipper;
            _upsShipper = upsShipper;
        }

        public IShipStuff BuildShip(bool withLivestock)
            => withLivestock ? _upsShipper : _fedExShipper;
    }
}