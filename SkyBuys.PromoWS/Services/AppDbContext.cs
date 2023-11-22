using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;
using System.Reflection;

namespace SkyBuys.PromoWS.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /*public DbSet<SkyBuysItem> SkyBuysItem { get; set; }*/
        public DbSet<SkyBuysPromo> SkyBuysPromo { get; set; }

    }
}
