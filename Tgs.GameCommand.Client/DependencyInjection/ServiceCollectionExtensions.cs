using Microsoft.Extensions.Options;
using Tgs.GameCommand.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameCommandClient(
        this IServiceCollection services,
        Action<GameCommandClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameCommandClientOptions>()
            .Configure(configure)
            .Services
            .AddHttpClient<IGameCommandClient, GameCommandClient>()
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameCommandClientOptions>>().Value;
                httpClient.BaseAddress = options.BaseUrl;
            })
            .Services;
}
