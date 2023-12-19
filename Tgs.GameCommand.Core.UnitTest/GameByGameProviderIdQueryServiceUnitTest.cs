using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameByGameProviderIdQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameRepository = Substitute.For<IGameRepository>();

        var query = new GameByGameProviderIdQuery(
            GameProviderId: Guid.NewGuid());

        var games = new GameInfo[] {
            new(
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
                "KY" )
        };
        _ = fakeGameRepository
            .QueryAsync(
                gameProviderId: Arg.Is(query.GameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(games.ToAsyncEnumerable());

        var service = new GameByGameProviderIdQueryService(
            fakeGameRepository);

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameInfo>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
