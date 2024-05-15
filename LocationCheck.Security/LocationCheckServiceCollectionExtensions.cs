using LocationCheck.Security.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LocationCheck.Security
{
    public static class LocationCheckServiceCollectionExtensions
    {
        //TODO: Replace all strings with constants

        public static IServiceCollection AddLocationCheckSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication("BasicAuthentication").
            AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
            ("BasicAuthentication", null);

            return services;
        }
    }
}
