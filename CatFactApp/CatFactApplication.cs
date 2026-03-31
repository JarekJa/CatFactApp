using CatFactApp.Services;
using CatFactApp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatFactApp
{
    public class CatFactApplication
    {
        private readonly ICatFactService _catFactService;
        private readonly IFileService _fileService;
        private readonly ILogger<CatFactApplication> _logger;

        public CatFactApplication(
            ICatFactService catFactService,
            IFileService fileService,
            ILogger<CatFactApplication> logger)
        {
            _catFactService = catFactService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("=== Aplikacja pobierająca fakty o kotach ===");

            Console.WriteLine("\nPobieranie faktu o kocie...");

            var catFact = await _catFactService.GetCatFactAsync();

            if (catFact != null && !string.IsNullOrEmpty(catFact.Fact))
            {
                Console.WriteLine($"\nOtrzymany fakt: {catFact.Fact}");
                Console.WriteLine($"Długość: {catFact.Length} znaków");

                var saved = await _fileService.AppendCatFactAsync(catFact);

                if (saved)
                {
                    Console.WriteLine("Fakt został zapisany pomyślnie");
                    _logger.LogInformation("Fakt zapisany pomyślnie");
                }
                else
                {
                    Console.WriteLine("Nie udało się zapisać faktu");
                    _logger.LogWarning("Nie udało się zapisać faktu");
                }
            }
            else
            {
                Console.WriteLine("Nie udało się pobrać faktu o kocie.");
                _logger.LogWarning("Nie udało się pobrać faktu z API");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
        }

        }
}
