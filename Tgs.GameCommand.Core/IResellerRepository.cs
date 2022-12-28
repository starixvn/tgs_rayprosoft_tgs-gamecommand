using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IResellerRepository
{
    ValueTask<ResellerStatus> GetStatusAsync(Guid resellerId);
}
