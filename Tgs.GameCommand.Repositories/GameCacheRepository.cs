using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class GameCacheRepository(
	IGameRepository repository,
	IMemoryCache memoryCache) : IGameRepository
{
	public async ValueTask<GameInfo?> GetAsync(
		int id,
		CancellationToken cancellationToken = default)
	{
		var key = string.Format("gamecommand:game:get:{0}", id);
		if (!memoryCache.TryGetValue(key, out GameInfo? info))
		{
			info = await repository
						.GetAsync(
						   id: id,
						   cancellationToken: cancellationToken)
						.ConfigureAwait(false);
			if (info is not null)
			{
				_ = memoryCache.Set(
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
		if (!memoryCache.TryGetValue(key, out int? value))
		{
			value = await repository.GetIdAsync(
				gameProviderId: gameProviderId,
				gameExternalId: gameExternalId,
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);

			if (value.HasValue)
			{
				return memoryCache.Set(
					key: key,
					value: value);
			}
		}
		return value;
	}

	public ValueTask<GameStatus> GetStatusAsync(
		int id,
		CancellationToken cancellationToken = default)
		=> repository.GetStatusAsync(id, cancellationToken);

	public IAsyncEnumerable<GameInfo> QueryAsync(
		Guid gameProviderId,
		CancellationToken cancellationToken = default)
		=> repository.QueryAsync(gameProviderId, cancellationToken);

	public IAsyncEnumerable<GameInfo> QueryAsync(
		Guid gameProviderId,
		GameType type,
		CancellationToken cancellationToken = default)
		=> repository.QueryAsync(gameProviderId, type, cancellationToken);
}
