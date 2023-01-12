using System.Runtime.CompilerServices;
using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderCodeQueryService : IQueryService<GameByGameProviderCodeQuery, GameInfo>
{
    private readonly IGameProviderRepository _gameProviderRepository;
    private readonly IGameRepository _gameRepository;

    public GameByGameProviderCodeQueryService(
        IGameProviderRepository gameProviderRepository,
        IGameRepository gameRepository)
    {
        _gameProviderRepository = gameProviderRepository ?? throw new ArgumentNullException(nameof(gameProviderRepository));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameProviderCodeQuery query,
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
