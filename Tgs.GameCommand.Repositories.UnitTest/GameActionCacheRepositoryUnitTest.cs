using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameActionCacheRepositoryUnitTest
{
    [TestMethod]
    public async Task Query()
    {
        var fakeRepository = Substitute.For<IGameActionRepository>();
        var fakeMemoryCache = Substitute.For<IMemoryCache>();

        var id = Guid.NewGuid();
        var key = string.Format("gamecommand:gameaction:query:{0}", id);

        _ = fakeMemoryCache
            .TryGetValue(
                key: Arg.Is(key),
                value: out _)
             .Returns(false);

        var gameActions = new GameActionInfo[] {
            new(
                Id: Guid.NewGuid(),
                RoundId: id,
                PlayerCode: string.Empty,
                ExternalTransactionId: Guid.NewGuid().ToString("N"),
                Type: GameActionType.PlaceBet,
                Amount: 1m,
                Turnover: 0m,
                WinLoss: 0m,
                UnsettledBets: 0m,
                Balance: 99m,
                BetType: string.Empty,
                ExternalBetId: string.Empty,
                ExternalGameActionGroupId: string.Empty,
                ReferencedGameActionId: null,
                GameActionBatchId: null,
                RecordedOn: DateTimeOffset.UtcNow,
                SentOn: DateTimeOffset.UtcNow,
                IsClosingRound: false,
                ClosingRoundExplicitRequest: null,
                PlayerIp: "127.0.0.1",
                ExtraData: null,
                JackpotContribution: null,
                StoredOn: DateTimeOffset.UtcNow)};
        _ = fakeRepository
            .QueryAsync(
                Arg.Is(id),
                Arg.Any<CancellationToken>())
            .Returns(gameActions.ToAsyncEnumerable());

        var repository = new GameActionCacheRepository(
            fakeRepository,
            fakeMemoryCache);

        var actual = await repository.QueryAsync(
            id)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameActionInfo>();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());

        _ = fakeMemoryCache
            .Received(1)
            .Set(
                key: key,
                gameActions);
    }
}
