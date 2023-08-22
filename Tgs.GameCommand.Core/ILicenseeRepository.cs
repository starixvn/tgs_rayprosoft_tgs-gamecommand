using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface ILicenseeRepository
{
    ValueTask<LicenseeStatus> GetStatusAsync(
        Guid licenseeId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<LicenseeInfo> QueryAsync(
        CancellationToken cancellationToken = default);
}
