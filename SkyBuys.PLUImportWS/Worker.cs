using SkyBuys.Enum.Enum;
using SkyBuys.Models;
using SkyBuys.PLUImportWS.Models;
using SkyBuys.PLUImportWS.Services;

namespace SkyBuys.PLUImportWS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ReadXml _readXml = new ReadXml();
        private readonly IItemDefinitionRepository _itemDefinitionRepository = new ItemDefinitionRepository();
        private string _currTime;

        //public Worker(ILogger<Worker> logger, IItemDefinitionRepository itemDefinitionRepository)
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            //_itemDefinitionRepository = itemDefinitionRepository;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                _currTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
                try
                {
                    if (Array.Find(GlobalStaticVaiables.FileExtractionTimes, element => element == _currTime) != null)
                    {
                        List<ItemDefinition> itemDefinitions = (List<ItemDefinition>)await _readXml.GetItemDefinition();
                        _itemDefinitionRepository.UpdateData(itemDefinitions);
                    };
                }
                catch (Exception ex)
                {
                    TextLogger.LogToText(LoogerType.Error, $"Error reading Worker Service execution times. Exception : {ex.Message}");
                }

                await Task.Delay(GlobalStaticVaiables.Interval, stoppingToken);
            }
        }
    }
}