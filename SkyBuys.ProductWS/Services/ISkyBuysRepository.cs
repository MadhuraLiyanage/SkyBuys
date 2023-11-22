using SkyBuys.Models;

namespace SkyBuys.ProductWS.Services
{
    public interface ISkyBuysRepository
    {
        IEnumerable<SkyBuysItem> GetSkyBuysItems();
    }
}
