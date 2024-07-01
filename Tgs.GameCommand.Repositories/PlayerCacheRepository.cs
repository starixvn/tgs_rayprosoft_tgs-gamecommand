using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class PlayerCacheRepository(
	IPlayerRepository repository,
	IMemoryCache memoryCache,
	TimeSpan absoluteExpirationRelativeToNow) : IPlayerRepository
{
	public async ValueTask<PlayerInfo?> GetAsync(
		string playerCode,
		CancellationToken cancellationToken = default)
	{
		var key = string.Format("gamecommand:player:get:{0}", playerCode);
		if (!memoryCache.TryGetValue(key, out PlayerInfo? playerInfo))
		{
			playerInfo = await repository.GetAsync(
				playerCode: playerCode,
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);
			if (playerInfo is not null)
			{
				_ = memoryCache.Set(
					key: key,
					value: playerInfo,
					absoluteExpirationRelativeToNow: absoluteExpirationRelativeToNow);
			}
		}
		return playerInfo;
	}

	public ValueTask<PlayerStatus?> GetStatusAsync(
		string playerCode,
		CancellationToken cancellationToken = default)
		=> repository.GetStatusAsync(
			playerCode: playerCode,
			cancellationToken: cancellationToken);

	public IAsyncEnumerable<PlayerInfo> QueryAsync(
		Guid brandId,
		int pageIndex = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default)
		=> repository.QueryAsync(
			brandId: brandId,
			pageIndex: pageIndex,
			pageSize: pageSize,
			cancellationToken: cancellationToken);
}
