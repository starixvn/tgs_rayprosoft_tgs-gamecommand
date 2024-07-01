using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameByGameProviderIdGameTypeQueryServiceUnitTest
{
	[Fact]
	public async Task Empty()
	{
		var fakeGameRepository = Substitute.For<IGameRepository>();

		var query = new GameByGameProviderIdGameTypeQuery(
			GameProviderId: Guid.NewGuid(),
			GameType: GameType.CardGame);

		_ = fakeGameRepository
			.QueryAsync(
				gameProviderId: Arg.Is(query.GameProviderId),
				type: Arg.Is(query.GameType),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Enumerable.Empty<GameInfo>().ToAsyncEnumerable());

		var service = new GameByGameProviderIdGameTypeQueryService(
			fakeGameRepository);

		var actual = await service.QueryAsync(
			query: query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameInfo>();

		Assert.NotNull(actual);
		Assert.False(actual.Any());
	}

	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameRepository = Substitute.For<IGameRepository>();

		var query = new GameByGameProviderIdGameTypeQuery(
			GameProviderId: Guid.NewGuid(),
			GameType: GameType.CardGame);

		var games = new GameInfo[] {
			new(
				GameId: 1,
				"baccent",
				query.GameProviderId,
				"KY",
				"1",
				GameType.CardGame,
				GamePlatformType.DesktopAndMobile,
				"Baccent",
				true,
				false,
				true,
				true,
				string.Empty,
				string.Empty,
				string.Empty,
				null,
				string.Empty,
				"KY" )
		};
		_ = fakeGameRepository
			.QueryAsync(
				gameProviderId: Arg.Is(query.GameProviderId),
				type: Arg.Is(query.GameType),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(games.ToAsyncEnumerable());

		var service = new GameByGameProviderIdGameTypeQueryService(
			fakeGameRepository);

		var actual = await service.QueryAsync(query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
	}
}
