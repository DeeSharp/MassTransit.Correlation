using MassTransit.Correlation.Business.Messages;

namespace MassTransit.Correlation.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _busControl;

        public Worker(ILogger<Worker> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting bus");
            await _busControl.StartAsync(stoppingToken);

            await Task.Delay(2000);

            _logger.LogInformation("Sending message");

            var firstMessage = new FirstMessage()
            {
                ConversationId = Guid.NewGuid(),
            };

            await _busControl.Publish(firstMessage, stoppingToken);
        }
    }
}