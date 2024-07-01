using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpectedObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
	[Fact]
	public async Task GameProvider_Get()
	{
		var gameProviderCode = "ky";

		var gameProviderId = Guid.NewGuid();

		var expected = new GameProviderInfo(
			GameProviderId: gameProviderId,
			GameProviderCode: gameProviderCode,
			UniqueId: 1,
			Name: gameProviderCode,
			TransferWallet: true,
			AutoCloseRound: false,
			SupportTestPlayer: false,
			AllowKickPlayer: true,
			PlaceBetTimeout: 0,
			SettleBetTimeout: 0,
			Description: string.Empty,
			Localization: null);

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeGameProviderRepository
				.GetAsync(
					id: Arg.Is(gameProviderId),
					cancellationToken: Arg.Any<CancellationToken>())
				.Returns(ValueTask.FromResult<GameProviderInfo?>(expected));

			_ = services.AddSingleton(fakeGameProviderRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetAsync(gameProviderId);

		Assert.NotNull(actual);
		expected.ToExpectedObject().ShouldEqual(actual);
	}

	[Fact]
	public async Task GameProvider_Get_CultureCode()
	{
		var gameProviderId = Guid.NewGuid();
		var cultureCode = "zh-CN";
		var gameProviderCultureCode = "CN";

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.GetCultureCodeAsync(
					gameProviderId,
					cultureCode,
					Arg.Any<CancellationToken>())
				.Returns(ValueTask.FromResult<string?>(gameProviderCultureCode));

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetCultureCodeAsync(gameProviderId, cultureCode);

		Assert.NotNull(actual);
		Assert.Equal(gameProviderCultureCode, actual);
	}

	[Fact]
	public async Task GameProvider_Get_Currency()
	{
		var gameProviderId = Guid.NewGuid();
		var currencyCode = "CNY";
		var gameProviderCurrencyCode = "RMB";
		var gameProviderCurrencyInfo = new Core.Models.GameProviderCurrencyModel(
			gameProviderId,
			currencyCode,
			gameProviderCurrencyCode
			);
		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.GetCurrencyAsync(
					gameProviderId,
					currencyCode,
					Arg.Any<CancellationToken>())
				.Returns(ValueTask.FromResult<GameProviderCurrencyModel?>(gameProviderCurrencyInfo));
			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetCurrencyAsync(gameProviderId, currencyCode);

		Assert.NotNull(actual);
		Assert.Equal(currencyCode, actual.CurrencyCode);
		Assert.Equal(gameProviderCurrencyCode, actual.GameProviderCurrencyCode);
	}

	[Fact]
	public async Task GameProvider_Get_CurrencyCode()
	{
		var gameProviderId = Guid.NewGuid();
		var currencyCode = "CNY";
		var gameProviderCurrencyCode = "RMB";

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.GetCurrencyCodeAsync(
					gameProviderId,
					currencyCode,
					Arg.Any<CancellationToken>())
				.Returns(ValueTask.FromResult<string?>(gameProviderCurrencyCode));
			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetCurrencyCodeAsync(gameProviderId, currencyCode);

		Assert.NotNull(actual);
		Assert.Equal(gameProviderCurrencyCode, actual);
	}

	[Fact]
	public async Task GameProvider_Get_GameProviderId()
	{
		var gameProviderCode = "ky";

		var gameProviderId = Guid.NewGuid();

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.GetGameProviderIdAsync(
					gameProviderCode,
					Arg.Any<CancellationToken>())
				.Returns(ValueTask.FromResult<Guid?>(gameProviderId));

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetGameProviderIdAsync(gameProviderCode);

		Assert.Equal(gameProviderId, actual);
	}

	[Fact]
	public async Task GameProvider_Get_GameProviderIds()
	{
		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			var gameProviderIds = new[]
			{
				new GameProviderIdInfo(
					GameProviderCode: "KY",
					GameProviderId: Guid.NewGuid()
				),
				new GameProviderIdInfo(
					GameProviderCode: "LY",
					GameProviderId: Guid.NewGuid()
				),
			};

			_ = fakeRepository
				.GetGameProviderIdsAsync(
					Arg.Any<CancellationToken>())
				.Returns(gameProviderIds.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut
			.GameProviderGetGameProviderIdsAsync()
			.ToArrayAsync()
			?? Enumerable.Empty<GameProviderIdInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
	}

	[Fact]
	public async Task GameProvider_Id_GetStatus()
	{
		var gameProviderId = Guid.NewGuid();

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.GetStatusAsync(
					gameProviderId,
					Arg.Any<CancellationToken>())
				.Returns(GameProviderStatus.Active);

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.GameProviderGetStatusAsync(gameProviderId);

		Assert.Equal(GameProviderStatus.Active, actual);
	}

	[Fact]
	public async Task GameProvider_Query()
	{
		var gameProviderId = Guid.NewGuid();

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IGameProviderRepository>();

			_ = fakeRepository
				.QueryAsync(
				 cancellationToken: Arg.Any<CancellationToken>())
				.Returns(new GameProviderInfo[] {
					new(
						GameProviderId: gameProviderId,
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
						Localization: null) }.ToAsyncEnumerable());

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut
			.GameProviderQueryAsync()
			.ToArrayAsync()
			?? Enumerable.Empty<GameProviderInfo>();

		Assert.True(actual.Any());
	}
}
