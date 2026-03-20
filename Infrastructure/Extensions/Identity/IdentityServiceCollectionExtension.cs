using Application.Abstractions.Identity;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Identity;

public static class IdentityServiceCollectionExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<AuthenticationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<PersistenceContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
