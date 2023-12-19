using Tgs.GameCommand.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegisterExtensions
{
    public static async Task<WebApplication> UseGameCommandRegister(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var gameProviderRepository = scope.ServiceProvider.GetRequiredService<IGameProviderRepository>();

            var gameProviderIdInfos = await gameProviderRepository
                .GetGameProviderIdsAsync()
                .ToArrayAsync()
                .ConfigureAwait(false);

            foreach (var gameProviderIdInfo in gameProviderIdInfos)
            {
                _ = await gameProviderRepository.GetGameProviderIdAsync(
                    gameProviderIdInfo.GameProviderCode)
                    .ConfigureAwait(false);
                _ = await gameProviderRepository.GetAsync(
                    gameProviderIdInfo.GameProviderId)
                    .ConfigureAwait(false);
            }

            var brandRepository = scope.ServiceProvider.GetRequiredService<IBrandRepository>();

            var brandCodes = await brandRepository
                .GetBrandCodesAsync()
                .ToArrayAsync()
                .ConfigureAwait(false);

            foreach (var brandCode in brandCodes)
            {
                _ = await brandRepository.GetBrandIdAsync(
                    brandCode: brandCode)
                    .ConfigureAwait(false);
            }
        }

        return app;
    }
}
