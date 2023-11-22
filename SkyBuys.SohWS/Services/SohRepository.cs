using Microsoft.EntityFrameworkCore;
using SkyBuys.Enum.Enum;
using SkyBuys.Models;using System;

namespace SkyBuys.SohWS.Services
{
    public class SohRepository : ISohRepository
    {
        private AppDbContext _appDbContext;

        /*public ItemDefinitionRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }*/

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(GlobalStaticVaiables.DbConnectionString);

            return optionsBuilder.Options;
        }


        /*private DbContextOptions<AppDbContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(GlobalStaticVaiables.DBConnection);
            return optionBuilder.Options;
        }*/
        public void DeleteSohData()
        {
            try
            {
                using (_appDbContext = new AppDbContext(GetAllOptions()))
                {
                    _appDbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Soh");
                    _appDbContext.SaveChanges();
                    TextLogger.LogToText(LoogerType.Error, "Existing SOH deleted successfully fromn SOH table");
                }
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error saving SOH details to database. Exception : {ex.Message}");
            }
        }

        public void UpdateData(List<Soh> sohs)
        {
            try
            {
                using (_appDbContext = new AppDbContext(GetAllOptions()))
                {
                    TextLogger.LogToText(LoogerType.Error, $"No of SOH from API : {sohs.Count}");
                    sohs.ForEach(i => _appDbContext.soh.Add(i));
                    _appDbContext.Database.ExecuteSqlRaw("UPDATE Soh Set SubinventoryCode='DWS' Where SubinventoryCode='DWH'");
                    _appDbContext.SaveChanges();
                    TextLogger.LogToText(LoogerType.Error, "SOH details successfully fetched");
                }
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error saving SOH details to database. Exception : {ex.Message}");
            }
        }
    }
}
