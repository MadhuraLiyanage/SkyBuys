﻿using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;

namespace SkyBuys.ProductWS.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SkyBuysItem> SkyBuysItem { get; set; }

    }
}
