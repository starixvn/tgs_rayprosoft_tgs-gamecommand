using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameByGameProviderCodeQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameByGameProviderCodeQuery(
            GameProviderCode: "KY");

        var gameProviderId = Guid.NewGuid();
        _ = fakeGameProviderRepository
            .GetGameProviderIdAsync(
                code: Arg.Is(query.GameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var games = new GameInfo[] {
            new(
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
                "KY" )
        };
        _ = fakeGameRepository
            .QueryAsync(
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(games.ToAsyncEnumerable());

        var service = new GameByGameProviderCodeQueryService(
            fakeGameProviderRepository,
            fakeGameRepository);

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameInfo>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
