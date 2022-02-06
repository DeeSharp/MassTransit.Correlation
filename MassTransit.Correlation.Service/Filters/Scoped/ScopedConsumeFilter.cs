using GreenPipes;

namespace MassTransit.Correlation.Service.Filters.Scoped;

public class ScopedConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly FlowParams _flowParams;

    public ScopedConsumeFilter(FlowParams flowParams)
    {
        _flowParams = flowParams;
    }

    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var flowId = context.Headers.Get<string>("Token");
        _flowParams.FlowId = flowId;
        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
       
    }
}