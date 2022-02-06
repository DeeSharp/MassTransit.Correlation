using GreenPipes;
using MassTransit.ConsumeConfigurators;
using Serilog;
using Serilog.Context;

namespace MassTransit.Correlation.Service.Filters;

public class SimpleConsumeFilter<TContext> : IFilter<TContext> where TContext : class, ConsumeContext
{


    public SimpleConsumeFilter()
    {

    }

    public async Task Send(TContext context, IPipe<TContext> next)
    {
        Log.Information($"Entered SimpleConsumeFilter with context: {context.GetType().Name} with CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId}");

        context.GetOrAddPayload(() => new SomePayload("hello"));

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("my-consume-filter");
    }
}

public class SimpleConsumePipeSpec<TConsumer> : IPipeSpecification<ConsumerConsumeContext<TConsumer>>
    where TConsumer : class
{
    public void Apply(IPipeBuilder<ConsumerConsumeContext<TConsumer>> builder)
    {
        builder.AddFilter(new SimpleConsumeFilter<ConsumerConsumeContext<TConsumer>>());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}


public class SimpleConsumeMessageFilter<TContext, TMessage> : IFilter<TContext>
    where TContext : class, ConsumeContext<TMessage>
    where TMessage : class
{
    public SimpleConsumeMessageFilter()
    {

    }

    public async Task Send(TContext context, IPipe<TContext> next)
    {
        LogContext.PushProperty("CorrelationId", context.CorrelationId);
        LogContext.PushProperty("ConversationId", context.ConversationId);
        LogContext.PushProperty("InitiatorId", context.InitiatorId);
        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("consume-filter");
    }
}

public class SimpleConsumeMessagePipeSpec<TConsumer, TMessage> : IPipeSpecification<ConsumerConsumeContext<TConsumer, TMessage>>
    where TConsumer : class
    where TMessage : class
{
    public void Apply(IPipeBuilder<ConsumerConsumeContext<TConsumer, TMessage>> builder)
    {
        builder.AddFilter(new SimpleConsumeMessageFilter<ConsumerConsumeContext<TConsumer, TMessage>, TMessage>());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}

public class SimpleConsumePipeSpecObserver : IConsumerConfigurationObserver
{
    public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator)
        where TConsumer : class
    {
        //configurator.AddPipeSpecification(new SimpleConsumePipeSpec<TConsumer>());
    }

    public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
        where TConsumer : class
        where TMessage : class
    {
        configurator.AddPipeSpecification(new SimpleConsumeMessagePipeSpec<TConsumer, TMessage>());
    }
}