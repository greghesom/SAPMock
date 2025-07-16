using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SAPMock.ServiceDefaults;

public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.Services.AddServiceDiscovery();
        
        return builder;
    }
}
