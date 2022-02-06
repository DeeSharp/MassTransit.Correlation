using Automatonymous;
using MassTransit.Correlation.Service;
using MassTransit.Correlation.Service.Consumers;
using MassTransit.Correlation.Service.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<FirstService>();
        services.AddTransient<SecondService>();
        services.AddTransient<ThirdService>();
        services.AddHostedService<Worker>();
        services.AddMassTransit();
    })
    .UseLogging()
    .Build();

await host.RunAsync();



