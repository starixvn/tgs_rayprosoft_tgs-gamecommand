using Microsoft.Extensions.Caching.Memory;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Repositories;

internal class GameActionCacheRepository(
	IGameActionRepository repository,
	IMemoryCache memoryCache) : IGameActionRepository
{
	public IAsyncEnumerable<GameActionInfo> QueryAsync(
		Guid id,
		CancellationToken cancellationToken = default)
	{
		var key = string.Format("gamecommand:gameaction:query:{0}", id);

		if (!memoryCache.TryGetValue(key, out IEnumerable<GameActionInfo>? value))
		{
			value = repository.QueryAsync(
				id: id,
				cancellationToken: cancellationToken)
				.ToEnumerable() ?? Enumerable.Empty<GameActionInfo>();

			if (value.Any())
			{
				_ = memoryCache.Set(
					key: key,
					value: value,
				   absoluteExpirationRelativeToNow: TimeSpan.FromSeconds(10));
			}
		}

		return (value ?? Enumerable.Empty<GameActionInfo>()).ToAsyncEnumerable();
	}
}
