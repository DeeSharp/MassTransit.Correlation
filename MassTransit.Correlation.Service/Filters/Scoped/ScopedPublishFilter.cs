using GreenPipes;

namespace MassTransit.Correlation.Service.Filters.Scoped;

public class ScopedPublishFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly FlowParams _flowParams;

    public ScopedPublishFilter(FlowParams flowParams)
    {
        _flowParams = flowParams;
    }
    public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        //if (!string.IsNullOrWhiteSpace(_flowParams.FlowId))
        //    context.Headers.Set("Token", _flowParams.FlowId);

        if (string.IsNullOrWhiteSpace(_flowParams.FlowId))
        {
            _flowParams.FlowId = Guid.NewGuid().ToString();
        }
        context.Headers.Set("Token", _flowParams.FlowId);
        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        
    }
}