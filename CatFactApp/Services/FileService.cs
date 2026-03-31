using CatFactApp.Models;
using CatFactApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CatFactApp.Services;

public class FileService : IFileService
{
    private readonly string _filePath;
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cat_facts.txt");
    }

    public async Task<bool> AppendCatFactAsync(CatFactResponse catFact)
    {
        try
        {
            if (catFact == null || string.IsNullOrWhiteSpace(catFact.Fact))
            {
                _logger.LogWarning("Próba zapisu pustego faktu");
                return false;
            }

            var lineToWrite = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Długość: {catFact.Length} znaków | {catFact.Fact}";

            await File.AppendAllLinesAsync(_filePath, new[] { lineToWrite }, Encoding.UTF8);

            _logger.LogInformation("Fakt dopisany do pliku: {FilePath}", _filePath);
            return true;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Błąd wejścia/wyjścia przy zapisie do pliku: {FilePath}", _filePath);
            return false;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Brak dostępu do pliku: {FilePath}", _filePath);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Nieznany błąd przy zapisie do pliku: {FilePath}", _filePath);
            return false;
        }
    }
}