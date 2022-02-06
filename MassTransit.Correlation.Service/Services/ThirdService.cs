using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Messages;

namespace MassTransit.Correlation.Service.Services;

public class ThirdService
{
    private readonly IBusControl _busControl;
    private readonly ILogger<ThirdService> _logger;

    public ThirdService(IBusControl busControl, ILogger<ThirdService> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }
    public Task Process(ThirdMessage thirdMessage)
    {
        _logger.LogInformation("Entered Process method of ThirdService");
        _logger.LogInformation("Ending processing chain");
        _logger.LogInformation("Exit ThirdService");
        _logger.LogInformation("-------------------------------------------------------------");
        return Task.CompletedTask;
    }
}