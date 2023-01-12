using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameByGameProviderCodeGameTypeQueryServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameByGameProviderCodeGameTypeQuery(
            GameProviderCode: "KY",
            GameType: GameType.CardGame);

        var gameProviderId = Guid.NewGuid();
        _ = fakeGameProviderRepository
            .GetGameProviderIdAsync(
                code: Arg.Is(query.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var games = new GameInfo[] {
            new GameInfo(
                GameId: 1,
                "baccent",
                gameProviderId,
                query.GameProviderCode,
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
                gameProviderId: Arg.Is(gameProviderId),
                type: Arg.Is(query.GameType),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(games.ToAsyncEnumerable());

        var service = new GameByGameProviderCodeGameTypeQueryService(
            fakeGameProviderRepository,
            fakeGameRepository);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }
}
