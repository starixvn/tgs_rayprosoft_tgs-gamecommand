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
public class GameCacheRepositoryUnitTest
{
	[TestMethod]
	public async Task Get()
	{
		var fakeRepository = Substitute.For<IGameRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var id = 1;
		var key = string.Format("gamecommand:game:get:{0}", id);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var game = new GameInfo(
			GameId: id,
			GameCode: "baccent",
			GameProviderId: Guid.NewGuid(),
			GameProviderCode: "KY",
			GameExternalId: "1",
			Type: GameType.CardGame,
			PlatformType: GamePlatformType.DesktopAndMobile,
			Name: "Baccarat",
			SubGame: true,
			AvailableToBrand: false,
			HasTurnoverOnTieBet: false,
			Html5: true,
			Description: "Baccarat",
			EndpointPath: string.Empty,
			RuleUrl: string.Empty,
			AvailableCurrencies: null,
			Localization: string.Empty,
			SubGameProviderCode: "KY"
			);
		_ = fakeRepository
			.GetAsync(
				Arg.Is(id),
				Arg.Any<CancellationToken>())
			.Returns(game);

		var repository = new GameCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetAsync(id)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(id, actual!.GameId);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				game);
	}

	[TestMethod]
	public async Task Get_Id()
	{
		var fakeRepository = Substitute.For<IGameRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var gameProviderId = Guid.NewGuid();
		var gameExternalId = "1";
		var key = string.Format("gamecommand:game:get:id:{0}:{1}", gameProviderId, gameExternalId);

		_ = fakeMemoryCache
			.TryGetValue(
				key: Arg.Is(key),
				value: out _)
			 .Returns(false);

		var gameId = 1;
		_ = fakeRepository
			.GetIdAsync(
				Arg.Is(gameProviderId),
				Arg.Is(gameExternalId),
				Arg.Any<CancellationToken>())
			.Returns(gameId);

		var repository = new GameCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		var actual = await repository
			.GetIdAsync(
				gameProviderId,
				gameExternalId)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(gameId, actual);

		_ = fakeMemoryCache
			.Received(1)
			.Set(
				key: key,
				gameId);
	}

	[TestMethod]
	public void Get_Status()
	{
		var fakeRepository = Substitute.For<IGameRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var id = 1;

		var repository = new GameCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .GetStatusAsync(
				id: id);

		_ = fakeRepository
			.Received(1)
			.GetStatusAsync(
				id: Arg.Is(id),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Query()
	{
		var fakeRepository = Substitute.For<IGameRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var gameProviderId = Guid.NewGuid();

		var repository = new GameCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .QueryAsync(
				gameProviderId: gameProviderId);

		_ = fakeRepository
			.Received(1)
			.QueryAsync(
				gameProviderId: Arg.Is(gameProviderId),
				cancellationToken: Arg.Any<CancellationToken>());
	}

	[TestMethod]
	public void Query_Type()
	{
		var fakeRepository = Substitute.For<IGameRepository>();
		var fakeMemoryCache = Substitute.For<IMemoryCache>();

		var gameProviderId = Guid.NewGuid();
		var type = GameType.CardGame;

		var repository = new GameCacheRepository(
			fakeRepository,
			fakeMemoryCache);

		_ = repository
			  .QueryAsync(
				gameProviderId: gameProviderId,
				type: type);

		_ = fakeRepository
			.Received(1)
			.QueryAsync(
				gameProviderId: Arg.Is(gameProviderId),
				type: Arg.Is(type),
				cancellationToken: Arg.Any<CancellationToken>());
	}
}
