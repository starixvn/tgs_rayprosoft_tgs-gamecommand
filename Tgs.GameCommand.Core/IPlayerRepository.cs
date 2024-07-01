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

	IAsyncEnumerable<PlayerInfo> QueryAsync(
		Guid brandId,
		int pageIndex = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default);
}
