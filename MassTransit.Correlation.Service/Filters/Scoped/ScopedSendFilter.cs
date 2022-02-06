using GreenPipes;
using Microsoft.Recognizers.Text.Matcher;

namespace MassTransit.Correlation.Service.Filters.Scoped;

public class ScopedSendFilter<T> :
    IFilter<SendContext<T>>
    where T : class
{
    readonly FlowParams _flowParams;

    public ScopedSendFilter(FlowParams flowParams)
    {
        _flowParams = flowParams;
    }

    public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        if (!string.IsNullOrWhiteSpace(_flowParams.FlowId))
            context.Headers.Set("Token", _flowParams.FlowId);

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}