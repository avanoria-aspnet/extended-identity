using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Identity;

public static class IdentityServiceCollectionExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        return services;
    }
}
