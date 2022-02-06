using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Messages;

namespace MassTransit.Correlation.Service.Services;

public class SecondService
{
    private readonly IBusControl _busControl;
    private readonly ILogger<SecondService> _logger;

    public SecondService(IBusControl busControl, ILogger<SecondService> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }

    public Task Process(SecondMessage firstEvent, ConsumeContext<SecondMessage> context)
    {
        _logger.LogInformation("Entered Process method of SecondService");
        var thirdMessage = new ThirdMessage();
        //context.Publish(thirdMessage);
        _busControl.Publish(thirdMessage);
        _logger.LogInformation("Exit SecondService");
        _logger.LogInformation("-------------------------------------------------------------");
        return Task.CompletedTask;
    }
}