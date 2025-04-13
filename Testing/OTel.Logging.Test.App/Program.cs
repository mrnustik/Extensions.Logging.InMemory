using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

using (logger.BeginScope(new Dictionary<string, object>
       {
           { "Property", 42 },
           { "Scope property", "Property" }
       }))
{
    using (logger.BeginScope("Structured scope with {Property}", 41))
    {
        logger.LogInformation("Logged message with {Property}", 40);
    }
}

await host.RunAsync();