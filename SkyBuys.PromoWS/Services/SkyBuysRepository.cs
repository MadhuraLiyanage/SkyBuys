using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;

namespace SkyBuys.PromoWS.Services
{
    public class SkyBuysRepository : ISkyBuysRepository
    {
        private AppDbContext _appDbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(GlobalStaticVaiables.DbConnectionString);

            return optionsBuilder.Options;
        }

        /*public IEnumerable<SkyBuysItem> GetSkyBuysItems()
        {
            List<SkyBuysItem> skyBuysItems;
            using (_appDbContext = new AppDbContext(GetAllOptions()))
            {
                skyBuysItems = _appDbContext.SkyBuysItem.FromSqlRaw("EXEC sp_MapItemDetailsForShyBuys").ToList();
            }
            return skyBuysItems;
        }*/

        public IEnumerable<SkyBuysPromo> GetSkyBuysPromo()
        {
            List<SkyBuysPromo> skyBuysPromos;
            using (_appDbContext = new AppDbContext(GetAllOptions()))
            {
                skyBuysPromos = _appDbContext.SkyBuysPromo.FromSqlRaw("EXEC sp_Promotions").ToList();
            }
            return skyBuysPromos;
        }
    }
}
