using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameExternalGetService : IGetService<GameExternalQuery, GameInfo?>
{
    private readonly IGameProviderRepository _gameProviderRepository;
    private readonly IGameRepository _gameRepository;

    public GameExternalGetService(
        IGameProviderRepository gameProviderRepository,
        IGameRepository gameRepository)
    {
        _gameProviderRepository = gameProviderRepository ?? throw new ArgumentNullException(nameof(gameProviderRepository));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async ValueTask<GameInfo?> GetAsync(GameExternalQuery query, CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _gameProviderRepository
             .GetGameProviderIdAsync(
                 code: query.GameProviderCode,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        if (!gameProviderId.HasValue)
        {
            return null;
        }

        var gameId = await _gameRepository
            .GetIdAsync(
                gameProviderId: gameProviderId.Value,
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
