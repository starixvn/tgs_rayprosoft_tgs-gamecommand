using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderIdGameTypeQueryService : IQueryService<GameByGameProviderIdGameTypeQuery, GameInfo>
{
    private readonly IGameRepository _gameRepository;

    public GameByGameProviderIdGameTypeQueryService(
        IGameRepository gameRepository)
    {
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameProviderIdGameTypeQuery query,
        CancellationToken cancellationToken = default)
        => _gameRepository
            .QueryAsync(
                gameProviderId: query.GameProviderId,
                type: query.GameType,
                cancellationToken: cancellationToken);
}
