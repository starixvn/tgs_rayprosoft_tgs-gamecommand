using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class ResellerRepository : IResellerRepository
{
    private readonly TgsContext _context;

    public ResellerRepository(TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ValueTask<ResellerStatus> GetStatusAsync(Guid resellerId)
        => _context.Resellers.AsQueryable()
        .Where(x => x.Id == resellerId)
        .Select(x => (ResellerStatus)x.Status)
        .ToAsyncEnumerable()
        .FirstOrDefaultAsync();
}
