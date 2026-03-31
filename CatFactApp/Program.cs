using CatFactApp;
using CatFactApp.Services;
using CatFactApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        var app = host.Services.GetRequiredService<CatFactApplication>();

        await app.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {

                    services.AddHttpClient<ICatFactService, CatFactService>();
                    services.AddSingleton<IFileService, FileService>();
                    services.AddSingleton<CatFactApplication>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Information);
                });
}