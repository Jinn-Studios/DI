using JinnDev.Shipping.Core;

namespace JinnDev.OrderBadDI
{
    public interface IBuildShips
    {
        IShipStuff BuildShip(bool withLivestock);
    }
}