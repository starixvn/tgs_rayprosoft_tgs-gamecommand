using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameGameProviderIdExternalGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameRepository = Substitute.For<IGameRepository>();

		var query = new GameGameProviderIdExternalQuery(
			GameProviderId: Guid.NewGuid(),
			GameExternalId: "1");

		var gameId = 1;
		_ = fakeGameRepository
			.GetIdAsync(
				gameProviderId: Arg.Is(query.GameProviderId),
				gameExternalId: Arg.Is(query.GameExternalId),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameId);

		var game = new GameInfo(
				1,
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
				"KY");

		_ = fakeGameRepository
			.GetAsync(
				id: Arg.Is(gameId),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(game);

		var service = new GameGameProviderIdExternalGetService(
			fakeGameRepository);

		var actual = await service.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(gameId, actual.GameId);
	}
}
