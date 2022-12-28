using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class LicenseeRepository : ILicenseeRepository
{
    private readonly TgsContext _context;

    public LicenseeRepository(TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ValueTask<LicenseeStatus> GetStatusAsync(Guid licenseeId)
        => _context.Licensees
        .Where(x => x.Id == licenseeId)
        .Select(x => (LicenseeStatus)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync();
}
