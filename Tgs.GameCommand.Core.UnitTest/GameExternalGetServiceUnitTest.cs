using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameExternalGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameExternalQuery(
            GameProviderCode: "KY",
            GameExternalId: "1");

        var gameProviderId = Guid.NewGuid();
        _ = fakeGameProviderRepository
            .GetGameProviderIdAsync(
                code: Arg.Is(query.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameId = 1;
        _ = fakeGameRepository
            .GetIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(query.GameExternalId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var game = new GameInfo(
                1,
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
                "KY");

        _ = fakeGameRepository
            .GetAsync(
                id: Arg.Is(gameId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(game);

        var service = new GameExternalGetService(
            fakeGameProviderRepository,
            fakeGameRepository);

        var actual = await service
            .GetAsync(
                query)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(gameId, actual.GameId);
    }
}
