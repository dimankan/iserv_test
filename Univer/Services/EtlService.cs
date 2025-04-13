using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;

public class EtlService : IEtlService
{
    private readonly UniversitiesContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EtlService> _logger;

    public EtlService(
        UniversitiesContext db,
        IHttpClientFactory httpClientFactory,
        ILogger<EtlService> logger)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task RunEtlProcess(int maxThreads = 5)
    {
        var countries = new List<string>
        {
            "Russian Federation", "United States", "Germany",
            "France", "United Kingdom", "Japan", "China",
            "Brazil", "Canada", "Australia"
        };

        var httpClient = _httpClientFactory.CreateClient();
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxThreads };

        await Parallel.ForEachAsync(countries, parallelOptions, async (country, ct) =>
        {
            try
            {
                var response = await httpClient.GetAsync(
                    $"http://universities.hipolabs.com/search?country={Uri.EscapeDataString(country)}", ct);

                if (!response.IsSuccessStatusCode) return;

                var content = await response.Content.ReadAsStringAsync(ct);
                var universities = JsonSerializer.Deserialize<List<UniversityApiResponse>>(content);

                if (universities == null || !universities.Any()) return;

                var entities = universities.Select(u => new University
                {
                    Country = u.Country,
                    Name = u.Name,
                    Websites = u.WebPages != null ? string.Join(";", u.WebPages) : null
                }).ToList();

                await _db.Universities.AddRangeAsync(entities, ct);
                await _db.SaveChangesAsync(ct);

                _logger.LogInformation("Processed {Count} universities for {Country}",
                    entities.Count, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing {Country}", country);
            }
        });
    }
}

public class UniversityApiResponse
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; }
    [JsonPropertyName("web_pages")] public List<string> WebPages { get; set; }
}