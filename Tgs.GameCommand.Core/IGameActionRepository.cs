using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IGameActionRepository
{
    IAsyncEnumerable<GameActionInfo> QueryAsync(Guid id, CancellationToken cancellationToken = default);
}
