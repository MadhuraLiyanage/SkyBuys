using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;


namespace SkyBuys.ImagesWS.Services
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

        public IEnumerable<SkyBuysItem> GetSkyBuysItems()
        {
            List<SkyBuysItem> skyBuysItems;
            using (_appDbContext = new AppDbContext(GetAllOptions()))
            {
                skyBuysItems = _appDbContext.SkyBuysItem.FromSqlRaw("EXEC sp_MapItemDetailsForShyBuys").ToList();
            }
            return skyBuysItems;
        }
    }
}
