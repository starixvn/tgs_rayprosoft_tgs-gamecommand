using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class PlayerRepository : IPlayerRepository
{
    private readonly TgsContext _context;

    public PlayerRepository(TgsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async ValueTask<PlayerInfo?> GetAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
        => await (from player in _context.Players.AsQueryable()
                  join brand in _context.Brands.AsQueryable()
                  on player.BrandId equals brand.Id
                  join licensee in _context.Licensees.AsQueryable()
                  on brand.LicenseeId equals licensee.Id
                  join reseller in _context.Resellers.AsQueryable()
                  on licensee.ResellerId equals reseller.Id
                  where player.PlayerCode.ToLower().Contains(playerCode.ToLower())
                  select new PlayerInfo(
                      reseller.Id,
                      reseller.Code,
                      licensee.Id,
                      licensee.Code,
                      brand.Id,
                      brand.Code,
                      player.PlayerCode,
                      player.Username,
                      player.DefaultWalletId,
                      player.DefaultWalletCurrencyCode
                      ))
        .FirstOrDefaultAsync<PlayerInfo>(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerStatus?> GetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
        => await (from player in _context.Players.AsQueryable()
                  where player.PlayerCode.ToLower().Contains(playerCode.ToLower())
                  select (PlayerStatus?)player.Status)
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
