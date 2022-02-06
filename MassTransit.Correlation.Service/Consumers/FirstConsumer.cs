using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Services;

namespace MassTransit.Correlation.Service.Consumers;

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
        _service.Process(context.Message, context);
        return Task.CompletedTask;
    }
}

