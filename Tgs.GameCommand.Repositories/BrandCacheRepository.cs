using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class BrandCacheRepository : IBrandRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBrandRepository _repository;

    public BrandCacheRepository(
        IBrandRepository repository,
        IMemoryCache memoryCache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async ValueTask<Guid?> GetBrandIdAsync(
        string brandCode,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("query:brand:get:brand:id:{0}", brandCode);
        if (!_memoryCache.TryGetValue(key, out Guid? brandId))
        {
            brandId = await _repository
                        .GetBrandIdAsync(
                           brandCode: brandCode,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (brandId.HasValue)
            {
                _ = _memoryCache.Set(
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
        => _repository
        .GetCurrencyAsync(
            brandId,
            currencyCode,
            cancellationToken);

    public ValueTask<BrandGameProviderInfo?> GetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => _repository
        .GetGameProviderConfigurationAsync(
            brandId,
            gameProviderId,
            cancellationToken);

    public ValueTask<Guid?> GetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => _repository
        .GetGameProviderConfigurationIdAsync(
            brandId,
            gameProviderId,
            cancellationToken);

    public ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => _repository
        .GetGameProviderStatusAsync(
            brandId,
            gameProviderId,
            cancellationToken);

    public ValueTask<GameStatus?> GetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default)
        => _repository
        .GetGameStatusAsync(
            brandId,
            gameId,
            cancellationToken);

    public ValueTask<BrandStatus?> GetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
        => _repository
        .GetStatusAsync(
            brandId,
            cancellationToken);
}
