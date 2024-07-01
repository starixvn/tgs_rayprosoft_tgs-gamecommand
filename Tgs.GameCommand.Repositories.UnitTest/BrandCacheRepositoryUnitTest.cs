using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class BrandCacheRepositoryUnitTest
{
	[TestMethod]
	public void Get_BrandCodes()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetBrandCodesAsync()
			  .ToEnumerable();

		_ = fakeRepository
			.Received(1)
			.GetBrandCodesAsync(
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public async Task Get_BrandId()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandCode = "GPI";
		var key = string.Format("gamecommand:brand:get:brand:id:{0}", brandCode);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var brandId = Guid.NewGuid();
		_ = fakeRepository
			.GetBrandIdAsync(
				Arg.Is(brandCode),
				Arg.Any<CancellationToken>())
			.Returns(brandId);

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetBrandIdAsync(
				brandCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual.HasValue);
		Assert.AreEqual(brandId, actual.Value);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				brandId);
	}

	[TestMethod]
	public void Get_Currency()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandId = Guid.NewGuid();
		var currencyCode = "CNY";

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetCurrencyAsync(
				brandId: brandId,
				currencyCode: currencyCode);

		_ = fakeRepository
			.Received(1)
			.GetCurrencyAsync(
				brandId: Arg.Is(brandId),
				currencyCode: Arg.Is(currencyCode),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Get_Game_Status()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandId = Guid.NewGuid();
		var gameId = 1;

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetGameStatusAsync(
				brandId: brandId,
				gameId: gameId);

		_ = fakeRepository
			.Received(1)
			.GetGameStatusAsync(
				brandId: Arg.Is(brandId),
				gameId: Arg.Is(gameId),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Get_GameProvider_Configuration()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandId = Guid.NewGuid();
		var gameProviderId = Guid.NewGuid();

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetGameProviderConfigurationAsync(
				brandId: brandId,
				gameProviderId: gameProviderId);

		_ = fakeRepository
			.Received(1)
			.GetGameProviderConfigurationAsync(
				brandId: Arg.Is(brandId),
				gameProviderId: Arg.Is(gameProviderId),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Get_GameProvider_Status()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandId = Guid.NewGuid();
		var gameProviderId = Guid.NewGuid();

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetGameProviderStatusAsync(
				brandId: brandId,
				gameProviderId: gameProviderId);

		_ = fakeRepository
			.Received(1)
			.GetGameProviderStatusAsync(
				brandId: Arg.Is(brandId),
				gameProviderId: Arg.Is(gameProviderId),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Get_Status()
	{
		var fakeRepository = Substitute.For<IBrandRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var brandId = Guid.NewGuid();

		var repository = new BrandCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetStatusAsync(
				brandId: brandId);

		_ = fakeRepository
			.Received(1)
			.GetStatusAsync(
				brandId: Arg.Is(brandId),
				cancellationToken: Arg.Any<CancellationToken>());
	}
}
