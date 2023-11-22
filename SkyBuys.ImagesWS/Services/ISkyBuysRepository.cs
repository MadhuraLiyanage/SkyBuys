using SkyBuys.Models;

namespace SkyBuys.ImagesWS.Services
{
    public interface ISkyBuysRepository
    {
        IEnumerable<SkyBuysItem> GetSkyBuysItems();
    }
}
