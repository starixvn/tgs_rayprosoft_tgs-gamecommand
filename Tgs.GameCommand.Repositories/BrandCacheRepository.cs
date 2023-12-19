using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class BrandCacheRepository(
    IBrandRepository repository,
    IMemoryCache memoryCache) : IBrandRepository
{
    public IAsyncEnumerable<string> GetBrandCodesAsync(
        CancellationToken cancellationToken = default)
        => repository.GetBrandCodesAsync(cancellationToken);

    public async ValueTask<Guid?> GetBrandIdAsync(
        string brandCode,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:brand:get:brand:id:{0}", brandCode);
        if (!memoryCache.TryGetValue(key, out Guid? brandId))
        {
            brandId = await repository.GetBrandIdAsync(
                brandCode: brandCode,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            if (brandId.HasValue)
            {
                return memoryCache.Set(
                    key: key,
                    value: brandId);
            }
        }
        return brandId;
    }

    public ValueTask<string?> GetCurrencyAsync(
        Guid brandId,
        string currencyCode,
        CancellationToken cancellationToken = default)
        => repository.GetCurrencyAsync(
            brandId: brandId,
            currencyCode: currencyCode,
            cancellationToken: cancellationToken);

    public ValueTask<BrandGameProviderInfo?> GetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => repository.GetGameProviderConfigurationAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken);

    public ValueTask<Guid?> GetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => repository.GetGameProviderConfigurationIdAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken);

    public ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => repository.GetGameProviderStatusAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken);

    public ValueTask<GameStatus?> GetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default)
        => repository.GetGameStatusAsync(
            brandId: brandId,
            gameId: gameId,
            cancellationToken: cancellationToken);

    public ValueTask<BrandStatus?> GetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
        => repository.GetStatusAsync(
            brandId: brandId,
            cancellationToken: cancellationToken);

    public IAsyncEnumerable<BrandInfo> QueryAsync(
        CancellationToken cancellationToken = default)
        => repository.QueryAsync(
            cancellationToken: cancellationToken);

    public IAsyncEnumerable<BrandGameProviderInfo> QueryGameProviderAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
        => repository.QueryGameProviderAsync(
            brandId: brandId,
            cancellationToken: cancellationToken);
}
