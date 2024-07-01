using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameExternalGetService(
	IGameProviderRepository gameProviderRepository,
	IGameRepository gameRepository) : IGetService<GameExternalQuery, GameInfo?>
{
	public async ValueTask<GameInfo?> GetAsync(GameExternalQuery query, CancellationToken cancellationToken = default)
	{
		var gameProviderId = await gameProviderRepository.GetGameProviderIdAsync(
			code: query.GameProviderCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!gameProviderId.HasValue)
		{
			return null;
		}

		var gameId = await gameRepository.GetIdAsync(
			gameProviderId: gameProviderId.Value,
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
