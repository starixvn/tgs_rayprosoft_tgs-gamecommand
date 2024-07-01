using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameGameProviderIdExternalGetService(
	IGameRepository gameRepository) : IGetService<GameGameProviderIdExternalQuery, GameInfo?>
{
	public async ValueTask<GameInfo?> GetAsync(GameGameProviderIdExternalQuery query, CancellationToken cancellationToken = default)
	{
		var gameId = await gameRepository.GetIdAsync(
			gameProviderId: query.GameProviderId,
			gameExternalId: query.GameExternalId,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !gameId.HasValue
			? null
			: await gameRepository.GetAsync(
				id: gameId.Value,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
