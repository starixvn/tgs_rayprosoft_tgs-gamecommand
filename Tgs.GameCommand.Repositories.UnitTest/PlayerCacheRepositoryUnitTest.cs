using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class PlayerCacheRepositoryUnitTest
{
	[TestMethod]
	public async Task Get()
	{
		var fakeRepository = Substitute.For<IPlayerRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var playerCode = "test";
		var key = string.Format("gamecommand:player:get:{0}", playerCode);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var player = new PlayerInfo(
			ResellerId: Guid.NewGuid(),
			ResellerCode: "TGS",
			LicenseeId: Guid.NewGuid(),
			LicenseeCode: "TGS",
			BrandId: Guid.NewGuid(),
			BrandCode: "GPI",
			PlayerCode: playerCode,
			Username: playerCode,
			DefaultWalletId: Guid.NewGuid(),
			DefaultWalletCurrencyCode: "CNY");
		_ = fakeRepository
			.GetAsync(
				Arg.Is(playerCode),
				Arg.Any<CancellationToken>())
			.Returns(player);

		var repository = new PlayerCacheRepository(
			fakeRepository,
			fakeMemoryCache,
			TimeSpan.FromDays(1));

		var actual = await repository
			.GetAsync(playerCode)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(playerCode, actual!.PlayerCode);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				player);
	}

	[TestMethod]
	public void Get_Status()
	{
		var fakeRepository = Substitute.For<IPlayerRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var playerCode = "test";

		var repository = new PlayerCacheRepository(
			fakeRepository,
			fakeMemoryCache,
			TimeSpan.FromDays(1));

		_ = repository
			  .GetStatusAsync(
				playerCode: playerCode);

		_ = fakeRepository
			.Received(1)
			.GetStatusAsync(
				playerCode: Arg.Is(playerCode),
				cancellationToken: Arg.Any<CancellationToken>());
	}
}
