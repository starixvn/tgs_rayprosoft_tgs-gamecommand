using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommand.Core;

public interface IGameRepository
{
    ValueTask<GameInfo?> GetAsync(
        int id,
        CancellationToken cancellationToken = default);

    ValueTask<int?> GetIdAsync(
        Guid gameProviderId,
        string gameExternalId,
        CancellationToken cancellationToken = default);

    ValueTask<GameStatus> GetStatusAsync(
        int id,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameQueryModel> QueryAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameQueryModel> QueryAsync(
        Guid gameProviderId,
        GameType type,
        CancellationToken cancellationToken = default);
}
