using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class GameProviderRepository : IGameProviderRepository
{
    private readonly TgsContext _context;

    public GameProviderRepository(
        TgsContext context)
        => _context = context ?? throw new ArgumentNullException(nameof(context));

    public ValueTask<GameProviderInfo?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Where(x => x.Id == id)
        .Select(x => new GameProviderInfo(
            x.Id,
            x.Description,
            x.AutoCloseRound,
            x.SupportTestPlayer,
            x.AllowKickPlayer,
            x.PlaceBetTimeout,
            x.SettleBetTimeout,
            x.UniqueId,
            x.TransferWallet,
            x.Code,
            x.Name,
            x.Localization
            ))
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);

    public ValueTask<string?> GetCultureCodeAsync(
        Guid gameProviderId,
        string cultureCode,
        CancellationToken cancellationToken = default)
        => _context.GameProviderLanguages.AsQueryable()
        .Where(x =>
                x.GameProviderId == gameProviderId &&
                x.CultureCode.ToLower() == cultureCode.ToLower())
        .Select(x => x.GameProviderCultureCode)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);

    public ValueTask<Core.Models.GameProviderCurrencyModel?> GetCurrencyAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
        => _context.GameProviderCurrencies.AsQueryable()
        .Where(x => x.GameProviderId == gameProviderId &&
                x.CurrencyCode.ToLower() == currencyCode.ToLower())
        .Select(x => new Core.Models.GameProviderCurrencyModel(
            x.GameProviderId,
            x.CurrencyCode,
            x.GameProviderCurrencyCode
            ))
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);

    public async ValueTask<string?> GetCurrencyCodeAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var data = await GetCurrencyAsync(
                  gameProviderId,
                  currencyCode,
                  cancellationToken)
            .ConfigureAwait(false);
        return data?.GameProviderCurrencyCode;
    }

    public ValueTask<Guid?> GetGameProviderIdAsync(
        string code,
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Where(x => x.Code.ToLower() == code.ToLower())
        .Select(x => (Guid?)x.Id)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);

    public IAsyncEnumerable<GameProviderIdInfo> GetGameProviderIdsAsync(
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Select(x => new GameProviderIdInfo(x.Code, x.Id))
        .ToAsyncEnumerable();

    public ValueTask<GameProviderStatus?> GetStatusAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Where(x => x.Code.ToLower() == gameProviderCode.ToLower())
        .Select(x => (GameProviderStatus?)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);

    public ValueTask<GameProviderStatus> GetStatusAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _context.GameProviders.AsQueryable()
        .Where(x => x.Id == id)
        .Select(x => (GameProviderStatus)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(cancellationToken);
}
