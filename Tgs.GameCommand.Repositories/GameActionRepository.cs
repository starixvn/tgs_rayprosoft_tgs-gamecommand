using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class GameActionRepository(
    BetContext context) : IGameActionRepository
{
    private readonly BetContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IAsyncEnumerable<GameActionInfo> QueryAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _context.GameActions.AsQueryable()
            .Where(x => x.RoundId == id)
            .Select(x => new GameActionInfo(
                x.Id,
                x.RoundId,
                x.PlayerCode,
                x.ExternalTransactionId,
                (GameActionType)x.Type,
                x.Amount,
                x.Turnover,
                x.WinLoss,
                x.UnsettledBets,
                x.Balance,
                x.BetType,
                x.ExternalBetId,
                x.ExternalGameActionGroupId,
                x.ReferencedGameActionId,
                x.GameActionBatchId,
                x.RecordedOn,
                x.SentOn,
                x.IsClosingRound,
                x.ClosingRoundExplicitRequest,
                x.PlayerIp,
                x.ExtraData,
                x.JackpotContribution,
                x.StoredOn
                ))
        .AsAsyncEnumerable();
}
