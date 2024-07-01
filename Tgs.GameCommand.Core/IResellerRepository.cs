using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IResellerRepository
{
	ValueTask<ResellerStatus> GetStatusAsync(
		Guid resellerId,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<ResellerInfo> QueryAsync(
		CancellationToken cancellationToken = default);
}
