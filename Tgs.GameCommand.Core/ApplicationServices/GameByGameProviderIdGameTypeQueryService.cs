using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderIdGameTypeQueryService(
	IGameRepository gameRepository) : IQueryService<GameByGameProviderIdGameTypeQuery, GameInfo>
{
	public IAsyncEnumerable<GameInfo> QueryAsync(
		GameByGameProviderIdGameTypeQuery query,
		CancellationToken cancellationToken = default)
		=> gameRepository.QueryAsync(
			gameProviderId: query.GameProviderId,
			type: query.GameType,
			cancellationToken: cancellationToken);
}
