using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameByGameProviderIdGameTypeQueryServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameByGameProviderIdGameTypeQuery(
            GameProviderId: Guid.NewGuid(),
            GameType: GameType.CardGame);

        var games = new GameInfo[] {
            new GameInfo(
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

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }

    [TestMethod]
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

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.Any());
    }
}
