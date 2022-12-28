using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IPlayerRepository
{
    ValueTask<PlayerInfo?> GetAsync(
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerStatus?> GetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default);
}
