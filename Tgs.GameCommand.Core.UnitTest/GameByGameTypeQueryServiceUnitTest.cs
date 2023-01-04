using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameByGameTypeQueryServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameByGameTypeQuery(
            GameProviderCode: "KY",
            GameType: GameType.CardGame);

        var gameProviderId = Guid.NewGuid();
        _ = fakeGameProviderRepository
            .GetGameProviderIdAsync(
                code: Arg.Is(query.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var games = new GameQueryModel[] {
            new GameQueryModel(
                1,
                "baccent",
                gameProviderId,
                query.GameProviderCode,
                "1",
                GameStatus.Active,
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

        var service = new GameByGameTypeQueryService(
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
