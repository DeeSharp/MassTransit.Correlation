using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Services;

namespace MassTransit.Correlation.Service.Consumers;

public class SecondConsumer : IConsumer<SecondMessage>
{
    private readonly ILogger<SecondConsumer> _logger;
    private readonly SecondService _service;

    public SecondConsumer(ILogger<SecondConsumer> logger, SecondService service)
    {
        _logger = logger;
        _service = service;
    }

    public Task Consume(ConsumeContext<SecondMessage> context)
    {
        _logger.LogInformation($"SecondConsumer CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId}");
        _service.Process(context.Message, context);
        return Task.CompletedTask;
    }
}