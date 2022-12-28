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

        if (!gameId.HasValue)
        {
            return null;
        }

        var model = await _gameRepository
            .GetAsync(
                id: gameId.Value,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return model is null
            ? null
            : new GameInfo(
                GameId: model.GameId,
                GameCode: model.GameCode,
                GameProviderId: model.GameProviderId,
                GameProviderCode: model.GameProviderCode,
                GameExternalId: model.GameExternalId,
                Type: model.Type,
                PlatformType: model.PlatformType,
                Name: model.Name,
                SubGame: model.SubGame,
                AvailableToBrand: model.AvailableToBrand,
                HasTurnoverOnTieBet: model.HasTurnoverOnTieBet,
                Html5: model.Html5,
                Description: model.Description,
                EndpointPath: model.EndpointPath,
                RuleUrl: model.RuleUrl,
                AvailableCurrencies: model.AvailableCurrencies,
                Localization: model.Localization,
                SubGameProviderCode: model.SubGameProviderCode);
    }
}
