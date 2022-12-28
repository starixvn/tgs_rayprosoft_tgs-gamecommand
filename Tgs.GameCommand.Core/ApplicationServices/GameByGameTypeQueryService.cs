using System.Runtime.CompilerServices;
using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameByGameTypeQueryService : IQueryService<GameByGameTypeQuery, GameInfo>
{
    private readonly IGameProviderRepository _gameProviderRepository;
    private readonly IGameRepository _gameRepository;

    public GameByGameTypeQueryService(
        IGameProviderRepository gameProviderRepository,
        IGameRepository gameRepository)
    {
        _gameProviderRepository = gameProviderRepository ?? throw new ArgumentNullException(nameof(gameProviderRepository));
        _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
    }

    public async IAsyncEnumerable<GameInfo> QueryAsync(
        GameByGameTypeQuery query,
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
            .ToEnumerable()
            .Select(model => new GameInfo(
                model.GameId,
                model.GameCode,
                model.GameProviderId,
                model.GameProviderCode,
                model.GameExternalId,
                model.Type,
                model.PlatformType,
                model.Name,
                model.SubGame,
                model.AvailableToBrand,
                model.HasTurnoverOnTieBet,
                model.Html5,
                model.Description,
                model.EndpointPath,
                model.RuleUrl,
                model.AvailableCurrencies,
                model.Localization,
                model.SubGameProviderCode));

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
            yield return result;
    }
}
