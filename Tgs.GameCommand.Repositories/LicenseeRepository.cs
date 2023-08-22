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

    public ValueTask<LicenseeStatus> GetStatusAsync(
        Guid licenseeId,
        CancellationToken cancellationToken = default)
        => _context.Licensees
        .Where(x => x.Id == licenseeId)
        .Select(x => (LicenseeStatus)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken);

    public IAsyncEnumerable<LicenseeInfo> QueryAsync(
        CancellationToken cancellationToken = default)
        => (from licensee in _context.Licensees.AsQueryable()
            join reseller in _context.Resellers.AsQueryable()
            on licensee.ResellerId equals reseller.Id
            where licensee.Status != (int)LicenseeStatus.Archived
            select new LicenseeInfo(
                reseller.Id,
                reseller.Code,
                licensee.Id,
                licensee.Code))
        .ToAsyncEnumerable();
}
