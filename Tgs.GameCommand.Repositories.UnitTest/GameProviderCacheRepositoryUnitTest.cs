using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.Models;
using Tgs.GameCommand.Repositories;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameProviderCacheRepositoryUnitTest
{
	[TestMethod]
	public async Task Get()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var id = Guid.NewGuid();
		var key = string.Format("gamecommand:gameprovider:get:{0}", id);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var gameProvider = new GameProviderInfo(
			GameProviderId: id,
			GameProviderCode: "KY",
			UniqueId: 1,
			Name: "KY",
			TransferWallet: true,
			AutoCloseRound: false,
			SupportTestPlayer: false,
			AllowKickPlayer: true,
			PlaceBetTimeout: 0,
			SettleBetTimeout: 0,
			Description: string.Empty,
			Localization: null);
		_ = fakeRepository
			.GetAsync(
				Arg.Is(id),
				Arg.Any<CancellationToken>())
			.Returns(gameProvider);

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetAsync(id)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(id, actual!.GameProviderId);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				gameProvider);
	}

	[TestMethod]
	public async Task Get_Culture_Code()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var gameProviderId = Guid.NewGuid();
		var cultureCode = "CN-ZH";
		var key = string.Format("gamecommand:gameprovider:get:culture:code:{0}:{1}", gameProviderId, cultureCode);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var gameCultureCode = "cn";
		_ = fakeRepository
			.GetCultureCodeAsync(
				Arg.Is(gameProviderId),
				Arg.Is(cultureCode),
				Arg.Any<CancellationToken>())
			.Returns(gameCultureCode);

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetCultureCodeAsync(
				gameProviderId,
				cultureCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(gameCultureCode, actual);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				gameCultureCode);
	}

	[TestMethod]
	public async Task Get_Currency()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var gameProviderId = Guid.NewGuid();
		var CurrencyCode = "CNY";
		var key = string.Format("gamecommand:gameprovider:get:currency:{0}:{1}", gameProviderId, CurrencyCode);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var gameProviderCurrency = new GameProviderCurrencyModel(
			GameProviderId: gameProviderId,
			CurrencyCode: CurrencyCode,
			GameProviderCurrencyCode: "1");
		_ = fakeRepository
			.GetCurrencyAsync(
				Arg.Is(gameProviderId),
				Arg.Is(CurrencyCode),
				Arg.Any<CancellationToken>())
			.Returns(gameProviderCurrency);

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetCurrencyAsync(
				gameProviderId,
				CurrencyCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(gameProviderCurrency.GameProviderCurrencyCode, actual!.GameProviderCurrencyCode);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				gameProviderCurrency);
	}

	[TestMethod]
	public async Task Get_Currency_Code()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var id = Guid.NewGuid();
		var currencyCode = "CNY";
		var key = string.Format("gamecommand:gameprovider:get:currency:code:{0}:{1}", id, currencyCode);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var gameCurrencyCode = "1";
		_ = fakeRepository
			.GetCurrencyCodeAsync(
				Arg.Is(id),
				Arg.Is(currencyCode),
				Arg.Any<CancellationToken>())
			.Returns(gameCurrencyCode);

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetCurrencyCodeAsync(
				id,
				currencyCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(gameCurrencyCode, actual);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				gameCurrencyCode);
	}

	[TestMethod]
	public async Task Get_GameProvider_Id()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var code = "KY";
		var key = string.Format("gamecommand:gameprovider:get:id:{0}", code);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var id = Guid.NewGuid();
		_ = fakeRepository
			.GetGameProviderIdAsync(
				Arg.Is(code),
				Arg.Any<CancellationToken>())
			.Returns(id);

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetGameProviderIdAsync(
				code)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(id, actual);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				id);
	}

	[TestMethod]
	public void Get_GameProvider_Ids()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetGameProviderIdsAsync();

		_ = fakeRepository
			.Received(1)
			.GetGameProviderIdsAsync(
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Get_Status()
	{
		var fakeRepository = Substitute.For<IGameProviderRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var id = Guid.NewGuid();

		var repository = new GameProviderCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetStatusAsync(id);

		_ = fakeRepository
			.Received(1)
			.GetStatusAsync(
				id: Arg.Is(id),
				cancellationToken: Arg.Any<CancellationToken>());
	}
}
