using GreenPipes;
using MassTransit.Correlation.Business.Messages;
using MassTransit.PublishPipeSpecifications;
using Serilog;

namespace MassTransit.Correlation.Service.Filters;

public class SimplePublishFilter : IFilter<PublishContext>
{
    public SimplePublishFilter()
    {

    }

    public async Task Send(PublishContext context, IPipe<PublishContext> next)
    {
        Log.Information($"Entered SimplePublishFilter with context: {context.GetType().Name} with CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId}");
        var carrier = "send-message-filter";

        if (context.TryGetPayload(out ConsumeContext cc))
        {
            // should occur on message B and it DOES, all good

            carrier += ",has-consume-context";

            if (cc.TryGetPayload(out SomePayload ccsp)) // never occurs, NOT GOOD :(
                carrier += ",has-some-payload:" + ccsp.Text;
        }

        context.Headers.Set("x-send-message-filter", carrier);


        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("my-send-filter");
    }
}


public class SimplePublishMessageFilter<TMessage> : IFilter<PublishContext<TMessage>> where TMessage : class
{
    public SimplePublishMessageFilter()
    {

    }

    public async Task Send(PublishContext<TMessage> context, IPipe<PublishContext<TMessage>> next)
    {
        //Log.Information($"Entered SimplePublishMessageFilter with context: {context.GetType().Name} with CorrelationId: {context.CorrelationId} and ConversationId: {context.ConversationId} and InitiatorId: {context.InitiatorId} for message: {typeof(TMessage).Name}");

        if (context.Headers.TryGetHeader("ConversationId", out object @value))
        {
            var conversationId = Guid.Parse(@value.ToString());
            //Log.Information($"SimplePublishMessageFilter context headers has ConversationId of: {conversationId}");
            context.ConversationId = conversationId;
        }
        else
        {
            if (context.Message is BaseMessage baseEvent && !context.ConversationId.HasValue)
            {
                context.ConversationId = baseEvent.ConversationId ?? Guid.NewGuid();
                context.Headers.Set("ConversationId", context.ConversationId.ToString());
            }
        }
        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("my-send-message-filter");
    }
}

public class SomePayload
{
    public SomePayload(string text)
    {
        Text = text;
    }

    public string Text { get; }
}





public class SimplePublishPipeSpec : IPipeSpecification<PublishContext>
{
    public void Apply(IPipeBuilder<PublishContext> builder)
    {
        builder.AddFilter(new SimplePublishFilter());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}


public class SimplePublishMessagePipeSpec<TMessage> : IPipeSpecification<PublishContext<TMessage>> where TMessage : class
{
    public void Apply(IPipeBuilder<PublishContext<TMessage>> builder)
    {
        builder.AddFilter(new SimplePublishMessageFilter<TMessage>());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}

class SimplePublishPipeSpecObserver : IPublishPipeSpecificationObserver
{
    public void MessageSpecificationCreated<TMessage>(IMessagePublishPipeSpecification<TMessage> specification)
        where TMessage : class
    {
        specification.AddPipeSpecification(new SimplePublishMessagePipeSpec<TMessage>());
    }
}