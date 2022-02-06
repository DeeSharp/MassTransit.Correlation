using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Services;

namespace MassTransit.Correlation.Service.Consumers;

public class FirstMessage : BaseMessage
{

}

public class FirstConsumer : IConsumer<FirstMessage>
{
    private readonly ILogger<FirstConsumer> _logger;
    private readonly FirstService _service;

    public FirstConsumer(ILogger<FirstConsumer> logger, FirstService service)
    {
        _logger = logger;
        _service = service;
    }

    public Task Consume(ConsumeContext<FirstMessage> context)
    {
        _logger.LogInformation($"FirstConsumer CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId}");
        _service.Process(context.Message);
        return Task.CompletedTask;
    }
}

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

