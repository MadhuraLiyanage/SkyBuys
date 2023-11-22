using Microsoft.EntityFrameworkCore;
using SkyBuys.Enum.Enum;
using SkyBuys.Models;

namespace SkyBuys.PLUImportWS.Services
{
    public class ItemDefinitionRepository : IItemDefinitionRepository
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

        public void UpdateData(List<ItemDefinition> itemDefinitions)
        {
            try
            {
                using (_appDbContext = new AppDbContext(GetAllOptions()))
                {
                    TextLogger.LogToText(LoogerType.Error, $"No of Item Definition after grouping : {itemDefinitions.Count}");
                    _appDbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE itemDefinitions");
                    TextLogger.LogToText(LoogerType.Error, "Existing Item definitions deleted successfully from ItemDefinition table");
                    itemDefinitions.ForEach(i => _appDbContext.itemDefinitions.Add(i));
                    _appDbContext.SaveChanges();
                    TextLogger.LogToText(LoogerType.Error, "Item definitions updated successfully");
                }
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error saving Item Definitions to database. Exception : {ex.Message}");
            }
        }
    }
}
