using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderIdQueryService(
	IGameRepository gameRepository) : IQueryService<GameByGameProviderIdQuery, GameInfo>
{
	public IAsyncEnumerable<GameInfo> QueryAsync(
		GameByGameProviderIdQuery query,
		CancellationToken cancellationToken = default)
		=> gameRepository.QueryAsync(
			gameProviderId: query.GameProviderId,
			cancellationToken: cancellationToken);
}
