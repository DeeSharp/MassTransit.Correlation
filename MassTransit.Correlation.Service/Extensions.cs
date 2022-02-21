using System.Reflection;
using Amazon;
using MassTransit.Correlation.Service.Filters;
using MassTransit.Correlation.Service.Filters.Scoped;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace MassTransit.Correlation.Service;

public static class Extensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, params Type[] types)
    {
        var assembliesWithConsumers = GetAssembliesWithConsumers(types);

        services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumers(assembliesWithConsumers.ToArray());
            x.SetKebabCaseEndpointNameFormatter();

            services.AddScoped<FlowParams>();


            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConnectConsumerConfigurationObserver(new SimpleConsumePipeSpecObserver());
                cfg.ConfigurePublish(ppc =>
                {
                    ppc.AddPipeSpecification(new SimplePublishPipeSpec());
                    ppc.ConnectPublishPipeSpecificationObserver(new SimplePublishPipeSpecObserver());
                });


                cfg.UseSendFilter(typeof(ScopedSendFilter<>), context);
                cfg.UsePublishFilter(typeof(ScopedPublishFilter<>), context);
                cfg.UseConsumeFilter(typeof(ScopedConsumeFilter<>), context);

                

                cfg.UseDelayedMessageScheduler();
                cfg.ConfigureEndpoints(context);
                cfg.Host("localhost", rmq =>
                {
                    rmq.Username("guest");
                    rmq.Password("guest");
                });
            });
            
        });
        return services;
    }
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            var appNameProperty = "Athena-DataIntegrations";
            var correlationIdProperty = "CorrelationId";
            loggerConfiguration.MinimumLevel.Is(LogEventLevel.Information);
            loggerConfiguration.WriteTo.Seq(serverUrl: "http://host.docker.internal:4040");
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.Enrich.WithProperty("AppName", appNameProperty);
            loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());
            //loggerConfiguration.Enrich.WithCorrelationId(correlationIdProperty);

            
        });
    }

    private static List<Assembly> GetAssembliesWithConsumers(params Type[] types)
    {
        var assembliesWithConsumers = new List<Assembly> { Assembly.GetEntryAssembly() };
        foreach (var type in types)
        {
            var assemblyFromType = Assembly.GetAssembly(type);
            if (assemblyFromType != null)
            {
                assembliesWithConsumers.Add(assemblyFromType);
            }
        }
        return assembliesWithConsumers;
    }
}