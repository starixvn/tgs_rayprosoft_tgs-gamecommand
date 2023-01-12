using System.Runtime.CompilerServices;
using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderCodeGameTypeQueryService : IQueryService<GameByGameProviderCodeGameTypeQuery, GameInfo>
{
    private readonly IGameProviderRepository _gameProviderRepository;
    private readonly IGameRepository _gameRepository;

    public GameByGameProviderCodeGameTypeQueryService(
        IGameProviderRepository gameProviderRepository,
        IGameRepository gameRepository)
    {
        _gameProviderRepository = gameProviderRepository ?? throw new ArgumentNullException(nameof(gameProviderRepository));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameProviderCodeGameTypeQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _gameProviderRepository
                    .GetGameProviderIdAsync(
                        code: query.GameProviderCode,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

        if (!gameProviderId.HasValue)
        {
            yield break;
        }

        var results = _gameRepository
            .QueryAsync(
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
