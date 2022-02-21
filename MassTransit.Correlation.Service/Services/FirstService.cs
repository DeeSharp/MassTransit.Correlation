using MassTransit.Correlation.Business.Messages;

namespace MassTransit.Correlation.Service.Services;

public class FirstService
{
    private readonly ILogger<FirstService> _logger;

    public FirstService(ILogger<FirstService> logger)
    {
        
        _logger = logger;
    }

    public Task Process( ConsumeContext<FirstMessage> consumeContext)
    {
        var secondMessage = new SecondMessage();
        consumeContext.Publish(secondMessage);
        return Task.CompletedTask;
    }
}