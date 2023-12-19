using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [Fact]
    public async Task GameAction_Query()
    {
        var roundId = Guid.NewGuid();

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameActionRepository>();

            _ = fakeRepository
            .QueryAsync(
                id: Arg.Is(roundId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns((new GameActionInfo[] {
                new(
                    Guid.NewGuid(),
                    roundId,
                    string.Empty,
                    Guid.NewGuid().ToString("N"),
                    GameActionType.PlaceBet,
                    1m,
                    0m,
                    0m,
                    0m,
                    99m,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    null,
                    null,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    false,
                    null,
                    "127.0.0.1",
                    null,
                    null,
                    DateTimeOffset.UtcNow)  }).ToAsyncEnumerable());
            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameActionQueryAsync(roundId);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(roundId, actual.First().RoundId);
        Assert.Equal(GameActionType.PlaceBet, actual.First().Type);
    }
}
