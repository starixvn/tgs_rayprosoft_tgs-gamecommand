using System.Runtime.CompilerServices;
using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderCodeGameTypeQueryService(
    IGameProviderRepository gameProviderRepository,
    IGameRepository gameRepository) : IQueryService<GameByGameProviderCodeGameTypeQuery, GameInfo>
{
    public async IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameProviderCodeGameTypeQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var gameProviderId = await gameProviderRepository.GetGameProviderIdAsync(
            code: query.GameProviderCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!gameProviderId.HasValue)
        {
            yield break;
        }

        var results = gameRepository.QueryAsync(
            gameProviderId: gameProviderId.Value,
            type: query.GameType,
            cancellationToken: cancellationToken)
            .ToEnumerable();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
            yield return result;
    }
}
