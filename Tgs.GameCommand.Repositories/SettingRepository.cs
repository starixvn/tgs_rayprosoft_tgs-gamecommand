using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class SettingRepository : ISettingRepository
{
    private readonly TgsContext _context;

    public SettingRepository(TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async ValueTask<string?> GetConfigurationAsync(string key)
        => (await _context.Settings
        .AsQueryable()
        .FirstOrDefaultAsync(x => x.Key.ToLower() == key.ToLower())
        .ConfigureAwait(false))?.Value;
}
