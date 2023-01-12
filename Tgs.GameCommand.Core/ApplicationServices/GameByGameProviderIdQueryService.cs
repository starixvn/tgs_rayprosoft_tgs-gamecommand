using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameProviderIdQueryService : IQueryService<GameByGameProviderIdQuery, GameInfo>
{
    private readonly IGameRepository _gameRepository;

    public GameByGameProviderIdQueryService(
        IGameRepository gameRepository)
    {
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameProviderIdQuery query,
        CancellationToken cancellationToken = default)
        => _gameRepository
        .QueryAsync(
            gameProviderId: query.GameProviderId,
            cancellationToken: cancellationToken);
}
