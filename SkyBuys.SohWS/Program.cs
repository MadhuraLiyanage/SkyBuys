using Microsoft.Extensions.Configuration;
using Serilog;
using SkyBuys.SohWS.Services;
using Microsoft.EntityFrameworkCore;
using SkyBuys.SohWS;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();


var section = config.GetSection(nameof(GlobalVariables));
var globalConfig = section.Get<GlobalVariables>();

GlobalStaticVaiables.DbConnectionString = config.GetConnectionString("DBConnection");
GlobalStaticVaiables.Interval = globalConfig.Interval;
GlobalStaticVaiables.LogFilePath = globalConfig.LogFilePath;
GlobalStaticVaiables.SohURI = globalConfig.SohURI;
GlobalStaticVaiables.SohMethod = globalConfig.SohMethod;
GlobalStaticVaiables.ApiToken = globalConfig.ApiToken;
GlobalStaticVaiables.OrganizationID = globalConfig.OrganizationID.Split(',');
GlobalStaticVaiables.SohExtractionTimes = globalConfig.SohExtractionTimes.Split(',');

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
    Log.Information("Starting up the SkyBuys SOH Extraction Worker Service");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error Starting SkyBuys SOH Extraction Worker Service");
    return;
}
finally
{
    Log.CloseAndFlush();
}


