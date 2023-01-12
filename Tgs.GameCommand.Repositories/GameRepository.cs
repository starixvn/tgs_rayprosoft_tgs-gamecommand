using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class GameRepository : IGameRepository
{
    private readonly TgsContext _context;

    public GameRepository(
       TgsContext context
       )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ValueTask<GameInfo?> GetAsync(
        int id,
        CancellationToken cancellationToken = default)
        => _context.Games
        .AsQueryable()
        .Where(x => x.Id == id)
        .Join(_context.GameProviders.AsQueryable(),
            game => game.GameProviderId,
            gameProvider => gameProvider.Id,
            (game, gameProvider) => new GameInfo(
                game.Id,
                game.Code,
                gameProvider.Id,
                gameProvider.Code,
                game.ExternalId,
                (GameType)game.Type,
                (GamePlatformType)game.PlatformType,
                game.Name,
                game.SubGame,
                game.AvailableToBrand,
                game.HasTurnoverOnTieBet,
                game.Html5,
                game.Description,
                game.EndpointPath,
                game.RuleUrl,
                (string.IsNullOrEmpty(game.AvailableCurrencies)) ? null : game.AvailableCurrencies.Split(",", StringSplitOptions.RemoveEmptyEntries),
                game.Localization,
                game.SubGameProviderCode)
            )
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public ValueTask<int?> GetIdAsync(
        Guid gameProviderId,
        string gameExternalId,
        CancellationToken cancellationToken = default)
        => _context.Games
        .AsQueryable()
        .Where(x => x.GameProviderId == gameProviderId
                && x.ExternalId == gameExternalId)
        .Select(game => (int?)game.Id)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public ValueTask<GameStatus> GetStatusAsync(
        int id,
        CancellationToken cancellationToken = default)
        => _context.Games
        .AsQueryable()
        .Where(x => x.Id == id)
        .Select(game => (GameStatus)game.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public IAsyncEnumerable<GameInfo> QueryAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Where(x => x.Id == gameProviderId)
        .Join(_context.Games.AsQueryable(),
            gameProvider => gameProvider.Id,
            game => game.GameProviderId,
            (gameProvider, game) => new GameInfo(
                game.Id,
                game.Code,
                gameProvider.Id,
                gameProvider.Code,
                game.ExternalId,
                (GameType)game.Type,
                (GamePlatformType)game.PlatformType,
                game.Name,
                game.SubGame,
                game.AvailableToBrand,
                game.HasTurnoverOnTieBet,
                game.Html5,
                game.Description,
                game.EndpointPath,
                game.RuleUrl,
                (string.IsNullOrEmpty(game.AvailableCurrencies)) ? null : game.AvailableCurrencies.Split(",", StringSplitOptions.RemoveEmptyEntries),
                game.Localization,
                game.SubGameProviderCode)
            )
        .ToAsyncEnumerable();

    public IAsyncEnumerable<GameInfo> QueryAsync(
        Guid gameProviderId,
        GameType type,
        CancellationToken cancellationToken = default)
        => _context.Games.AsQueryable()
        .Where(x => x.GameProviderId == gameProviderId && x.Type == (int)type)
        .Join(_context.GameProviders.AsQueryable(),
            game => game.GameProviderId,
            gameProvider => gameProvider.Id,
            (game, gameProvider) => new GameInfo(
                game.Id,
                game.Code,
                gameProvider.Id,
                gameProvider.Code,
                game.ExternalId,
                (GameType)game.Type,
                (GamePlatformType)game.PlatformType,
                game.Name,
                game.SubGame,
                game.AvailableToBrand,
                game.HasTurnoverOnTieBet,
                game.Html5,
                game.Description,
                game.EndpointPath,
                game.RuleUrl,
                (string.IsNullOrEmpty(game.AvailableCurrencies)) ? null : game.AvailableCurrencies.Split(",", StringSplitOptions.RemoveEmptyEntries),
                game.Localization,
                game.SubGameProviderCode))
        .ToAsyncEnumerable();
}
