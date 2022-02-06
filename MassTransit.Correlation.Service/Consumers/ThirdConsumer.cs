using MassTransit.Correlation.Business.Messages;
using MassTransit.Correlation.Service.Messages;
using MassTransit.Correlation.Service.Services;

namespace MassTransit.Correlation.Service.Consumers;

public class ThirdConsumer : IConsumer<ThirdMessage>
{
    private readonly ILogger<ThirdConsumer> _logger;
    private readonly ThirdService _service;

    public ThirdConsumer(ILogger<ThirdConsumer> logger, ThirdService service)
    {
        _logger = logger;
        _service = service;
    }
    public Task Consume(ConsumeContext<ThirdMessage> context)
    {
        _logger.LogInformation($"ThirdConsumer CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId}");
        _service.Process(context.Message);
        return Task.CompletedTask;
    }
}


