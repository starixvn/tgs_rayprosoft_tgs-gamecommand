using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class BrandRepository(TgsContext context) : IBrandRepository
{
	public IAsyncEnumerable<string> GetBrandCodesAsync(
		CancellationToken cancellationToken = default)
		=> context.Brands.AsQueryable()
		.Select(x => x.Code)
		.AsAsyncEnumerable();

	public async ValueTask<Guid?> GetBrandIdAsync(
		string brandCode,
		CancellationToken cancellationToken = default)
		=> (await context.Brands.FirstOrDefaultAsync(
			b => b.Code == brandCode,
			cancellationToken: cancellationToken)
		.ConfigureAwait(false))?.Id;

	public ValueTask<string?> GetCurrencyAsync(
		Guid brandId,
		string currencyCode,
		CancellationToken cancellationToken = default)
		=> context.BrandCurrencies.AsQueryable()
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
		=> (from brand in context.Brands.AsQueryable()
			join brandGameProvider in context.BrandGameProviders.AsQueryable()
			on brand.Id equals brandGameProvider.BrandId
			join gameProvider in context.GameProviders.AsQueryable()
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
		=> (await context.BrandGameProviders.AsQueryable()
		.FirstOrDefaultAsync(
					x => x.GameProviderId == gameProviderId && x.BrandId == brandId,
					cancellationToken: cancellationToken)
		.ConfigureAwait(false))?.GameProviderConfigurationId;

	public ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
		Guid brandId,
		Guid gameProviderId,
		CancellationToken cancellationToken = default)
		=> (from brand in context.Brands.AsQueryable()
			join brandGameProvider in context.BrandGameProviders.AsQueryable()
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
		=> (from brand in context.Brands.AsQueryable()
			join brandGame in context.BrandGames.AsQueryable()
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
		=> context.Brands
		.Where(x => x.Id == brandId)
		.Select(x => (BrandStatus?)x.Status)
		.ToAsyncEnumerable()
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken);

	public IAsyncEnumerable<BrandInfo> QueryAsync(
		CancellationToken cancellationToken = default)
		=> (from brand in context.Brands.AsQueryable()
			join licensee in context.Licensees.AsQueryable()
			on brand.LicenseeId equals licensee.Id
			join reseller in context.Resellers.AsQueryable()
			on licensee.ResellerId equals reseller.Id
			where brand.Status != (int)BrandStatus.Archived
			select new BrandInfo(
				reseller.Id,
				reseller.Code,
				licensee.Id,
				licensee.Code,
				brand.Id,
				brand.Code))
		.ToAsyncEnumerable();

	public IAsyncEnumerable<BrandGameProviderInfo> QueryGameProviderAsync(
		Guid brandId,
		CancellationToken cancellationToken = default)
		=> (from brand in context.Brands.AsQueryable()
			join brandGameProvider in context.BrandGameProviders.AsQueryable()
			on brand.Id equals brandGameProvider.BrandId
			join gameProvider in context.GameProviders.AsQueryable()
			on brandGameProvider.GameProviderId equals gameProvider.Id
			select new BrandGameProviderInfo(
				brand.Id,
				brand.Code,
				gameProvider.Id,
				gameProvider.Code,
				brandGameProvider.GameProviderConfigurationId,
				brandGameProvider.DefaultLanguageCode,
				brandGameProvider.DesktopQueryString,
				brandGameProvider.MobileQueryString))
		.ToAsyncEnumerable();
}
