using MassTransit.Correlation.Business.Messages;

namespace MassTransit.Correlation.Service.Services;

public class FirstService
{
    private readonly IBusControl _busControl;
    private readonly ILogger<FirstService> _logger;

    public FirstService(IBusControl busControl, ILogger<FirstService> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }

    public Task Process(FirstMessage firstMessage)
    {
        var secondMessage = new SecondMessage();
        _busControl.Publish(secondMessage);
        return Task.CompletedTask;
    }
}