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

        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var catFactService = host.Services.GetRequiredService<ICatFactService>();
        var fileService = host.Services.GetRequiredService<IFileService>();

        logger.LogInformation("=== Aplikacja pobierająca fakty o kotach ===");

        Console.WriteLine("\nPobieranie faktu o kocie...");

        var catFact = await catFactService.GetCatFactAsync();

        if (catFact != null && !string.IsNullOrEmpty(catFact.Fact))
        {
            Console.WriteLine($"\nOtrzymany fakt: {catFact.Fact}");
            Console.WriteLine($"Długość: {catFact.Length} znaków");

            var saved = await fileService.AppendCatFactAsync(catFact);

            if (saved)
            {
                Console.WriteLine("Fakt został zapisany pomyślnie");
                logger.LogInformation("Fakt zapisany pomyślnie");
            }
            else
            {
                Console.WriteLine("Nie udało się zapisać faktu");
                logger.LogWarning("Nie udało się zapisać faktu");
            }
        }
        else
        {
            Console.WriteLine("Nie udało się pobrać faktu o kocie.");
            logger.LogWarning("Nie udało się pobrać faktu z API");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć...");
        Console.ReadKey();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient<ICatFactService, CatFactService>();
                services.AddSingleton<IFileService, FileService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Information);
            });
}