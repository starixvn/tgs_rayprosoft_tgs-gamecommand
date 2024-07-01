using Tgs.GameCommand.Common;

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

	IAsyncEnumerable<GameInfo> QueryAsync(
		Guid gameProviderId,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameInfo> QueryAsync(
		Guid gameProviderId,
		GameType type,
		CancellationToken cancellationToken = default);
}
