using Serilog;
using SkyBuys.ImagesWS.Services;
using Microsoft.EntityFrameworkCore;
using SkyBuys.ImagesWS;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();


var section = config.GetSection(nameof(GlobalVariables));
var globalConfig = section.Get<GlobalVariables>();

GlobalStaticVaiables.DbConnectionString = config.GetConnectionString("DBConnection");
GlobalStaticVaiables.Interval = globalConfig.Interval;
GlobalStaticVaiables.LogFilePath = globalConfig.LogFilePath;
GlobalStaticVaiables.SkyBuysFilePath = globalConfig.SkyBuysFilePath;
GlobalStaticVaiables.SkyBuysApiBaseUrl = globalConfig.SkyBuysApiBaseUrl;
GlobalStaticVaiables.SkyBuysApiLoginEndpoint = globalConfig.SkyBuysApiLoginEndpoint;
GlobalStaticVaiables.SkyBuysImageEndpoint = globalConfig.SkyBuysImageEndpoint;
GlobalStaticVaiables.SkyBuysApiLoginName = globalConfig.SkyBuysApiLoginName;
GlobalStaticVaiables.SkyBuysApiPassword = SkyBuys.Utilities.Common.Decrypt(globalConfig.SkyBuysApiPassword);
GlobalStaticVaiables.RunOnScheduledTimes = globalConfig.RunOnScheduledTimes.Split(',');
GlobalStaticVaiables.ExcludeTimeRange = globalConfig.ExcludeTimeRange.Split(',');

//logger
Log.Logger = new LoggerConfiguration().WriteTo.File(GlobalStaticVaiables.LogFilePath, rollingInterval: RollingInterval.Day).CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices( services =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(GlobalStaticVaiables.DbConnectionString);
        services.AddScoped<AppDbContext>(db => new AppDbContext(optionsBuilder.Options));

        services.AddHostedService<Worker>();
    })
    .UseWindowsService()
    .Build();


try
{
    Log.Information("Starting up the SkyBuys Image Uploading Worker Service");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error Starting SkyBuys SkyBuys Image Uploading Worker Service");
    return;
}
finally
{
    Log.CloseAndFlush();
}


