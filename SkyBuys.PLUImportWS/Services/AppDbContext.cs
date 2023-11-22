using Microsoft.EntityFrameworkCore;
using SkyBuys.Models;
using System.Reflection;

namespace SkyBuys.PLUImportWS.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ItemDefinition> itemDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed Item Definition table for testing
            modelBuilder.Entity<ItemDefinition>().Property(i => i.RecId).ValueGeneratedOnAdd();
            modelBuilder.Entity<ItemDefinition>().HasData(
                new ItemDefinition
                {
                    RecId = -1,
                    ItemNumber = "ITM1",
                    ShortDescription = "TEST ITEM 1",
                    LongDescription = "TEST ITEM SHORT DESCRIPTION",
                    MainCategory = "MAIN CATEGORY",
                    SubCategory = "SUB CATEGORY",
                    Brand = "BRAND",
                    ItemSize = "SIZE"

                });

            //Seed Soh
            modelBuilder.Entity<Soh>().Property(i => i.RecId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Soh>().HasData(
                new Soh
                {
                    RecId = -1,
                    OrganizationCode = "NADAPR",
                    SubinventoryCode = "NAD",
                    ItemNumber = "1111111",
                    PrimaryQuantity = 10
                });

            //Seed Sub Inventory
            //modelBuilder.Entity<SubInventory>().Property(i => i.RecId).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubInventory>().HasData(
                new SubInventory
                {
                    SubInventoryCode = "DFA",
                    SubInventoryDescription = "Nadi Tappoo Shop - Arrival",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "DFV",
                    SubInventoryDescription = "Nadi Tappoo Vodafone Shop - Arrival",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "DCS",
                    SubInventoryDescription = "Nadi Cuppabula Convenience Store - Arrival",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "DFD",
                    SubInventoryDescription = "Nadi Tappoo Shop - Departure",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "DNK",
                    SubInventoryDescription = "Nadi Tappoo Nike Shop - Departure",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "DWS",
                    SubInventoryDescription = "Nadi Tappoo W/Smith - Departure",
                    Active = true
                },
                new SubInventory
                {
                    SubInventoryCode = "NSD",
                    SubInventoryDescription = "Nausori Tappoo Shop - Departure",
                    Active = true
                });
            /*
             *  database migration
                Command : Add-Migration InitialCreate
                
                Updating the Database
                Command : Update-Database
            */
        }

    }
}
