using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class BrandRepository : IBrandRepository
{
    private readonly TgsContext _context;

    public BrandRepository(TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async ValueTask<Guid?> GetBrandIdAsync(
        string brandCode,
        CancellationToken cancellationToken = default)
                => (await _context.Brands
            .FirstOrDefaultAsync(
                    b => b.Code == brandCode,
                    cancellationToken: cancellationToken)
            .ConfigureAwait(false))?.Id;

    public ValueTask<string?> GetCurrencyAsync(
        Guid brandId,
        string currencyCode,
        CancellationToken cancellationToken = default)
        => _context.BrandCurrencies.AsQueryable()
        .Where(x => x.BrandId == brandId
                 && x.CurrencyCode == currencyCode)
        .Select(x => x.BrandCurrencyCode)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public ValueTask<BrandGameProviderInfo?> GetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => (from brand in _context.Brands.AsQueryable()
            join brandGameProvider in _context.BrandGameProviders.AsQueryable()
            on brand.Id equals brandGameProvider.BrandId
            join gameProvider in _context.GameProviders.AsQueryable()
            on brandGameProvider.GameProviderId equals gameProvider.Id
            where brand.Id == brandId && gameProvider.Id == gameProviderId
            select new BrandGameProviderInfo(
                brand.Id,
                brand.Code,
                gameProvider.Id,
                gameProvider.Code,
                brandGameProvider.GameProviderConfigurationId,
                brandGameProvider.DefaultLanguageCode,
                brandGameProvider.DesktopQueryString,
                brandGameProvider.MobileQueryString))
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
                cancellationToken: cancellationToken);

    public async ValueTask<Guid?> GetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => (await _context.BrandGameProviders.AsQueryable()
        .FirstOrDefaultAsync(
                    x => x.GameProviderId == gameProviderId && x.BrandId == brandId,
                    cancellationToken: cancellationToken)
        .ConfigureAwait(false))?.GameProviderConfigurationId;

    public ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
        => (from brand in _context.Brands.AsQueryable()
            join brandGameProvider in _context.BrandGameProviders.AsQueryable()
            on brand.Id equals brandGameProvider.BrandId
            where
            brand.Id == brandId &&
            brandGameProvider.GameProviderId == gameProviderId
            select (GameProviderStatus?)brandGameProvider.Status
           )
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public ValueTask<GameStatus?> GetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default)
        => (from brand in _context.Brands.AsQueryable()
            join brandGame in _context.BrandGames.AsQueryable()
            on brand.Id equals brandGame.BrandId
            where
            brand.Id == brandId &&
            brandGame.GameId == gameId
            select (GameStatus?)brandGame.Status
           )
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public ValueTask<BrandStatus?> GetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
        => _context.Brands
        .Where(x => x.Id == brandId)
        .Select(x => (BrandStatus?)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);
}
