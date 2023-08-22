using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommand.Repositories;

internal class GameProviderCacheRepository : IGameProviderRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IGameProviderRepository _repository;

    public GameProviderCacheRepository(
        IGameProviderRepository repository,
        IMemoryCache memoryCache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async ValueTask<GameProviderInfo?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:gameprovider:get:{0}", id);
        if (!_memoryCache.TryGetValue(key, out GameProviderInfo? model))
        {
            model = await _repository
                .GetAsync(
                    id: id,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            if (model is not null)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: model,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
            }
        }
        return model;
    }

    public async ValueTask<string?> GetCultureCodeAsync(
        Guid gameProviderId,
        string cultureCode,
        CancellationToken
        cancellationToken = default)
    {
        var key = string.Format("gamecommand:gameprovider:get:culture:code:{0}:{1}", gameProviderId, cultureCode);
        if (!_memoryCache.TryGetValue(key, out string? gameCultureCode))
        {
            gameCultureCode = await _repository
                        .GetCultureCodeAsync(
                           gameProviderId: gameProviderId,
                           cultureCode: cultureCode,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (!string.IsNullOrEmpty(gameCultureCode))
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: gameCultureCode,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
            }
        }
        return gameCultureCode;
    }

    public async ValueTask<GameProviderCurrencyModel?> GetCurrencyAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:gameprovider:get:currency:{0}:{1}", gameProviderId, currencyCode);
        if (!_memoryCache.TryGetValue(key, out GameProviderCurrencyModel? model))
        {
            model = await _repository
                        .GetCurrencyAsync(
                           gameProviderId: gameProviderId,
                           currencyCode: currencyCode,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (model is not null)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: model,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
            }
        }
        return model;
    }

    public async ValueTask<string?> GetCurrencyCodeAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:gameprovider:get:currency:code:{0}:{1}", gameProviderId, currencyCode);
        if (!_memoryCache.TryGetValue(key, out string? gameCurrencyCode))
        {
            gameCurrencyCode = await _repository
                        .GetCurrencyCodeAsync(
                           gameProviderId: gameProviderId,
                           currencyCode: currencyCode,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (!string.IsNullOrEmpty(gameCurrencyCode))
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: gameCurrencyCode,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
            }
        }
        return gameCurrencyCode;
    }

    public async ValueTask<Guid?> GetGameProviderIdAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:gameprovider:get:id:{0}", code);
        if (!_memoryCache.TryGetValue(key, out Guid? id))
        {
            id = await _repository
                        .GetGameProviderIdAsync(
                           code: code,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (id.HasValue)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: id);
            }
        }
        return id;
    }

    public IAsyncEnumerable<GameProviderIdInfo> GetGameProviderIdsAsync(CancellationToken cancellationToken = default)
        => _repository.GetGameProviderIdsAsync(
            cancellationToken: cancellationToken);

    public ValueTask<GameProviderStatus> GetStatusAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository
        .GetStatusAsync(
            id: id,
            cancellationToken: cancellationToken);

    public IAsyncEnumerable<GameProviderInfo> QueryAsync(CancellationToken cancellationToken = default)
        => _repository
        .QueryAsync(
            cancellationToken: cancellationToken);
}
