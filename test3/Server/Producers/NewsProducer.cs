using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSocketServer.Producers
{
    public class NewsProducer : BackgroundService
    {
        private readonly ILogger<NewsProducer> _logger;
        private readonly TopicConfiguration _config;

        public NewsProducer(ILogger<NewsProducer> logger, TopicConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string message = string.Format(_config.MessageFormat, TopicGenerator.GetRandomTopic());
                await BroadcastService.BroadcastMessage(message, _config.Name);
                _logger.LogInformation($"News Producer: {message}");
                await Task.Delay(_config.Interval, stoppingToken);
            }
        }
    }
}
