using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class PlayerCacheRepository : IPlayerRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IPlayerRepository _repository;

    public PlayerCacheRepository(
        IPlayerRepository repository,
        IMemoryCache memoryCache
        )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async ValueTask<PlayerInfo?> GetAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var key = string.Format("\"game:command:player:get:{0}", playerCode);
        if (!_memoryCache.TryGetValue(key, out PlayerInfo? playerInfo))
        {
            playerInfo = await _repository
                        .GetAsync(
                           playerCode: playerCode,
                           cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            if (playerInfo is not null)
            {
                _ = _memoryCache.Set(
                    key: key,
                    value: playerInfo,
                    absoluteExpirationRelativeToNow: TimeSpan.FromDays(1));
            }
        }
        return playerInfo;
    }

    public ValueTask<PlayerStatus?> GetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
        => _repository
        .GetStatusAsync(
            playerCode: playerCode,
            cancellationToken: cancellationToken);
}
