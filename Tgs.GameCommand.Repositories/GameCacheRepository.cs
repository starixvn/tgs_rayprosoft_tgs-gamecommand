using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommand.Repositories;

internal class GameCacheRepository : IGameRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IGameRepository _repository;

    public GameCacheRepository(
        IGameRepository repository,
        IMemoryCache memoryCache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async ValueTask<GameInfo?> GetAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:game:get:{0}", id);
        if (!_memoryCache.TryGetValue(key, out GameInfo? info))
        {
            info = await _repository
                        .GetAsync(
                           id: id,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (info is not null)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: info,
                    absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(1));
            }
        }
        return info;
    }

    public async ValueTask<int?> GetIdAsync(
        Guid gameProviderId,
        string gameExternalId,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("gamecommand:game:get:id:{0}:{1}", gameProviderId, gameExternalId);
        if (!_memoryCache.TryGetValue(key, out int? value))
        {
            value = await _repository
                        .GetIdAsync(
                           gameProviderId: gameProviderId,
                           gameExternalId: gameExternalId,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (value.HasValue)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: value);
            }
        }
        return value;
    }

    public ValueTask<GameStatus> GetStatusAsync(
        int id,
        CancellationToken cancellationToken = default)
        => _repository.GetStatusAsync(id, cancellationToken);

    public IAsyncEnumerable<GameQueryModel> QueryAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => _repository.QueryAsync(gameProviderId, cancellationToken);

    public IAsyncEnumerable<GameQueryModel> QueryAsync(
        Guid gameProviderId,
        GameType type,
        CancellationToken cancellationToken = default)
        => _repository.QueryAsync(gameProviderId, type, cancellationToken);
}
