using SkyBuys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.PromoWS.Services
{
    public interface ISkyBuysRepository
    {
        /*IEnumerable<SkyBuysItem> GetSkyBuysItems();*/
        IEnumerable<SkyBuysPromo> GetSkyBuysPromo();
    }
}
