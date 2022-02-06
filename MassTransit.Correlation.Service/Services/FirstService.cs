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

    public Task Process(FirstMessage firstEvent, ConsumeContext<FirstMessage> consumeContext)
    {
        _logger.LogInformation("Entered Process method of FirstService");
        var secondMessage = new SecondMessage();
        //consumeContext.Publish(secondMessage);
        _busControl.Publish(secondMessage);
        _logger.LogInformation("Exit FirstService");
        _logger.LogInformation("-------------------------------------------------------------");
        return Task.CompletedTask;
    }
}