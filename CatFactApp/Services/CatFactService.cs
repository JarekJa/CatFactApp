using CatFactApp.Models;
using CatFactApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CatFactApp.Services
{
    public class CatFactService : ICatFactService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatFactService> _logger;
        private readonly string _apiUrl;

        public CatFactService(HttpClient httpClient, ILogger<CatFactService> logger, AppConfig config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrl = config.ApiUrl;
        }
        public async Task<CatFactResponse?> GetCatFactAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var catFact = JsonSerializer.Deserialize<CatFactResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return catFact;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd HTTP podczas pobierania faktu");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Błąd deserializacji JSON");
                return null;
            }
            catch (OperationCanceledException )
            {
                _logger.LogWarning("Operacja została anulowana");
                return null;
            }
        }
    }
}
