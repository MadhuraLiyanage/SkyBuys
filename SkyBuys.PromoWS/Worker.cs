using SkyBuys.Enum.Enum;

namespace SkyBuys.PromoWS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ProcessSkyBuysFile _processSkyBuysFile = new ProcessSkyBuysFile();
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
                _currTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);

                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

                //check on time range to exclude the execution
                if (GlobalStaticVaiables.ExcludeTimeRange[0] == "" || GlobalStaticVaiables.ExcludeTimeRange.Length == 0)
                {
                    Execute();
                }
                else
                {
                    bool ruProcess = true;
                    foreach (string schedule in GlobalStaticVaiables.ExcludeTimeRange)
                    {
                        string[] times = schedule.Split('-');
                        double startTime = double.Parse(times[0].Replace(":","."));
                        double endTime = double.Parse(times[1].Replace(":", "."));
                        if ((startTime <= double.Parse(_currTime.Replace(":",".")) && (endTime >= double.Parse(_currTime.Replace(":", ".")))))
                        {
                            ruProcess = false;
                        }

                    }

                    if (ruProcess)
                    {
                        Execute();
                    }
                }
                await Task.Delay(GlobalStaticVaiables.Interval, stoppingToken);
            }
        }

        private void Execute()
        {
            try
            {
                if (GlobalStaticVaiables.RunOnScheduledTimes.Length == 0 || GlobalStaticVaiables.RunOnScheduledTimes[0].Length == 0)
                {
                    _processSkyBuysFile.SendSkyBuysFile();
                }
                else
                {
                    if (Array.Find(GlobalStaticVaiables.RunOnScheduledTimes, element => element == _currTime) != null)
                    {
                        _processSkyBuysFile.SendSkyBuysFile();
                    }
                }

            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error reading Worker Service execution times. Exception : {ex.Message}");
            }
        }
    }
}