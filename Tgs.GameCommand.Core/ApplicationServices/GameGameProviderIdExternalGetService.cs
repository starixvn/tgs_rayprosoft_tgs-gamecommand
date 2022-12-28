using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameGameProviderIdExternalGetService : IGetService<GameGameProviderIdExternalQuery, GameInfo?>
{
    private readonly IGameRepository _gameRepository;

    public GameGameProviderIdExternalGetService(
        IGameRepository gameRepository)
    {
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async ValueTask<GameInfo?> GetAsync(GameGameProviderIdExternalQuery query, CancellationToken cancellationToken = default)
    {
        var gameId = await _gameRepository
            .GetIdAsync(
                gameProviderId: query.GameProviderId,
                gameExternalId: query.GameExternalId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !gameId.HasValue
            ? null
            : await _gameRepository
            .GetAsync(
                id: gameId.Value,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
