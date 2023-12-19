using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class PlayerRepository(TgsContext context) : IPlayerRepository
{
    public async ValueTask<PlayerInfo?> GetAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
        => await (from player in context.Players.AsQueryable()
                  join brand in context.Brands.AsQueryable()
                  on player.BrandId equals brand.Id
                  join licensee in context.Licensees.AsQueryable()
                  on brand.LicenseeId equals licensee.Id
                  join reseller in context.Resellers.AsQueryable()
                  on licensee.ResellerId equals reseller.Id
                  where player.PlayerCode.ToLower() == playerCode.ToLower()
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
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerStatus?> GetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
        => await (from player in context.Players.AsQueryable()
                  where player.PlayerCode.ToLower() == playerCode.ToLower()
                  select (PlayerStatus?)player.Status)
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public IAsyncEnumerable<PlayerInfo> QueryAsync(
        Guid brandId,
        int pageIndex = 1,
        int pageSize = 1000,
        CancellationToken cancellationToken = default)
        => (from player in context.Players.AsQueryable()
            join brand in context.Brands.AsQueryable()
            on player.BrandId equals brand.Id
            join licensee in context.Licensees.AsQueryable()
            on brand.LicenseeId equals licensee.Id
            join reseller in context.Resellers.AsQueryable()
            on licensee.ResellerId equals reseller.Id
            where player.BrandId == brandId && player.Status != 3
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
        .Skip(pageSize * (pageIndex - 1))
        .Take(pageSize)
        .ToAsyncEnumerable();
}
