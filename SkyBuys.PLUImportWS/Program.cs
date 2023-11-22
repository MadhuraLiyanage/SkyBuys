using Microsoft.Extensions.Configuration;
using Serilog;
using SkyBuys.PLUImportWS.Services;
using Microsoft.EntityFrameworkCore;
using SkyBuys.PLUImportWS;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();


var section = config.GetSection(nameof(GlobalVariables));
var globalConfig = section.Get<GlobalVariables>();

GlobalStaticVaiables.DbConnectionString = config.GetConnectionString("DBConnection");
GlobalStaticVaiables.Interval = globalConfig.Interval;
GlobalStaticVaiables.LogFilePath = globalConfig.LogFilePath;
GlobalStaticVaiables.FtpUrl = globalConfig.FtpUrl;
GlobalStaticVaiables.FtpFileName = globalConfig.FtpFileName;
GlobalStaticVaiables.XMLFIlePath = globalConfig.XMLFIlePath;
GlobalStaticVaiables.FileExtractionTimes = globalConfig.FileExtractionTimes.Split(',');
GlobalStaticVaiables.FtpUserId = globalConfig.FtpUserId;
GlobalStaticVaiables.FtpPassword = SkyBuys.Utilities.Common.Decrypt(globalConfig.FtpPassword);
GlobalStaticVaiables.Domain = globalConfig.Domain;

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
    Log.Information("Starting up the SkyBuys FTP File Extraction Worker Service");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error Starting SkyBuys FTP File Extraction Worker Service");
    return;
}
finally
{
    Log.CloseAndFlush();
}


