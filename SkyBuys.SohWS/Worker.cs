using SkyBuys.Enum.Enum;
using SkyBuys.Models;
using SkyBuys.SohWS.Services;

namespace SkyBuys.SohWS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ReadSoh _readSoh = new ReadSoh();
        private readonly ISohRepository _sohRepository = new SohRepository();
        private string _currTime;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

                try
                {
                    _currTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
                    if (Array.Find(GlobalStaticVaiables.SohExtractionTimes, element => element == _currTime) != null)
                    {
                        _sohRepository.DeleteSohData();
                        foreach (string ordID in GlobalStaticVaiables.OrganizationID)
                        {
                            List<Soh> sohs = (List<Soh>)await _readSoh.GetSoh(ordID);
                            if (sohs != null)
                            {
                                _sohRepository.UpdateData(sohs);
                            }
                            else
                            {
                                TextLogger.LogToText(LoogerType.Error, $"No data extracted from Fusion");
                            }
                        }
                    }

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