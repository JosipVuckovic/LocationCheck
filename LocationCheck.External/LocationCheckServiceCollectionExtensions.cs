using LocationCheck.External.Constants;
using LocationCheck.External.GoogleMapsPlatform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocationCheck.External
{
    public static class LocationCheckServiceCollectionExtensions
    {
        public static IServiceCollection AddLocationCheckExternal(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleApiSettings>(configuration.GetSection(StringConstants.ApiSettings));
            services.AddTransient<ApiKeyHandler>();

            services.AddHttpClient<IGoogleMapsPlatformClient, GoogleMapsPlatformClient>(client => {
                client.BaseAddress = new Uri(configuration.GetValue<string>(StringConstants.ApiSettingsBaseUrl));                
            })
            .AddHttpMessageHandler<ApiKeyHandler>();

            return services;
        }
    }
}
