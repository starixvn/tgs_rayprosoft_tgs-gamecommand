using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameProviderStatusGetService(IGameProviderRepository repository) : IGetService<GameProviderStatusQuery, GameProviderStatus?>
{
	public async ValueTask<GameProviderStatus?> GetAsync(GameProviderStatusQuery query, CancellationToken cancellationToken = default)
	{
		var id = await repository.GetGameProviderIdAsync(
			code: query.Code,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !id.HasValue
			? null
			: await repository.GetStatusAsync(
				id: id.Value,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
