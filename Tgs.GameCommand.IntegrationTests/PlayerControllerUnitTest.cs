using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
	[Fact]
	public async Task Player_Get()
	{
		var resellerId = Guid.NewGuid();
		var resellerCode = "TGS";
		var licenseeId = Guid.NewGuid();
		var licenseeCode = "TGS";
		var brandId = Guid.NewGuid();
		var brandCode = "GPI";
		var playerCode = "4ucmqplxb029";

		await using var web = CreateTestSettingSite(services =>
		{
			var fakePlayerRepository = Substitute.For<IPlayerRepository>();

			_ = fakePlayerRepository
			.GetAsync(
				playerCode: Arg.Is(playerCode),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult<PlayerInfo?>(
				new PlayerInfo(
					 resellerId,
					 resellerCode,
					 licenseeId,
					 licenseeCode,
					 brandId,
					 brandCode,
					 playerCode,
					 playerCode,
					 Guid.NewGuid(),
					 "CNY")));

			_ = services.AddSingleton(fakePlayerRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.PlayerGetAsync(playerCode);

		Assert.NotNull(actual);
		Assert.Equal(resellerId, actual.ResellerId);
		Assert.Equal(licenseeId, actual.LicenseeId);
		Assert.Equal(brandId, actual.BrandId);
		Assert.Equal(playerCode, actual.PlayerCode);
	}

	[Fact]
	public async Task Player_Get_Status()
	{
		var playerCode = "4ucmqplxb029";

		await using var web = CreateTestSettingSite(services =>
		{
			var fakeRepository = Substitute.For<IPlayerRepository>();

			_ = fakeRepository.GetStatusAsync(playerCode)
				.Returns(ValueTask.FromResult<PlayerStatus?>(PlayerStatus.Enabled));

			_ = services.AddSingleton(fakeRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.PlayerGetStatusAsync(playerCode);

		Assert.Equal(PlayerStatus.Enabled, actual);
	}

	[Fact]
	public async Task Player_Query()
	{
		var brandId = Guid.NewGuid();
		int pageIndex = 1;
		int pageSize = 1000;

		await using var web = CreateTestSettingSite(services =>
		{
			var fakePlayerRepository = Substitute.For<IPlayerRepository>();

			var playerInfos = new PlayerInfo[]
			{
				new(
					ResellerId: Guid.NewGuid(),
					 ResellerCode: "TGS",
					 LicenseeId: Guid.NewGuid(),
					 LicenseeCode: "TGS",
					 BrandId: brandId,
					 BrandCode: "GPI",
					 PlayerCode: "4ucmqplxb029",
					 Username: "4ucmqplxb029",
					 DefaultWalletId: Guid.NewGuid(),
					 DefaultWalletCurrencyCode: "CNY")
			};

			_ = fakePlayerRepository
			.QueryAsync(
				brandId: Arg.Is(brandId),
				pageIndex: Arg.Is(pageIndex),
				pageSize: Arg.Is(pageSize),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(playerInfos.ToAsyncEnumerable());

			_ = services.AddSingleton(fakePlayerRepository);
		});

		var httpClient = web.CreateClient();

		var sut = new GameCommandClient(httpClient);

		var actual = await sut.PlayerQueryAsync(
			brandId: brandId,
			pageIndex: pageIndex,
			pageSize: pageSize)
			.ToArrayAsync()
			?? Enumerable.Empty<PlayerInfo>();
		;

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal(brandId, actual.First().BrandId);
	}
}
