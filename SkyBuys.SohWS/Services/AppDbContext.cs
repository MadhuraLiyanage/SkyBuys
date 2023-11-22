using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;
using System.Reflection;

namespace SkyBuys.SohWS.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Soh> soh { get; set; }


    }
}
