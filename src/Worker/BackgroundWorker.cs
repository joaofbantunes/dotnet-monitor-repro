namespace Worker;

public class BackgroundWorker : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BackgroundWorker> _logger;

    public BackgroundWorker(IHttpClientFactory httpClientFactory, ILogger<BackgroundWorker> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var client = _httpClientFactory.CreateClient(nameof(BackgroundWorker));
            
            var tasks = Enumerable
                .Range(0, 1000)
                .Select(_ => client.GetAsync("/", stoppingToken));

            var results = await Task.WhenAll(tasks);

            if (results.All(r => r.IsSuccessStatusCode))
            {
                _logger.LogInformation("All requests succeeded");
            }
            else
            {
                _logger.LogWarning("Requests failed");
            }
            
            //await Task.Delay(1000, stoppingToken);
        }
    }
}
