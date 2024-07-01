using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameActionRepositoryUnitTest
{
	[TestMethod]
	public async Task Round_Query()
	{
		var options = new DbContextOptionsBuilder<BetContext>()
			.UseInMemoryDatabase(databaseName: "bet")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new BetContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeRepository = new GameActionRepository(context);
		var roundId = Guid.NewGuid();
		var round = new GameAction()
		{
			Id = Guid.NewGuid(),
			RoundId = roundId,
			PlayerCode = string.Empty,
			ExternalTransactionId = Guid.NewGuid().ToString("N"),
			Type = (short)GameActionType.PlaceBet,
			Amount = 1m,
			Turnover = 0m,
			WinLoss = 0m,
			UnsettledBets = 0m,
			Balance = 99m,
			BetType = string.Empty,
			ExternalBetId = string.Empty,
			ExternalGameActionGroupId = string.Empty,
			ReferencedGameActionId = null,
			GameActionBatchId = null,
			RecordedOn = DateTime.UtcNow,
			SentOn = DateTime.UtcNow,
			IsClosingRound = false,
			ClosingRoundExplicitRequest = null,
			PlayerIp = "127.0.0.1",
			ExtraData = null,
			JackpotContribution = null,
			StoredOn = DateTime.UtcNow,
		};

		_ = await context.GameActions.AddAsync(round).ConfigureAwait(false);
		_ = await context.SaveChangesAsync().ConfigureAwait(false);
		var actual = await fakeRepository.QueryAsync(roundId).ToArrayAsync().ConfigureAwait(false);
		Assert.IsNotNull(actual);
		Assert.AreEqual(roundId, actual.First().RoundId);
		Assert.AreEqual(GameActionType.PlaceBet, actual.First().Type);
	}
}
