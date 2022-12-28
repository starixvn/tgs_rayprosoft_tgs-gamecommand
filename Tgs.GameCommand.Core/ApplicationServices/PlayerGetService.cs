using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class PlayerGetService : IGetService<PlayerQuery, PlayerInfo?>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerGetService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
    }

    public async ValueTask<PlayerInfo?> GetAsync(PlayerQuery query, CancellationToken cancellationToken = default)
    {
        var player = await _playerRepository
            .GetAsync(
                playerCode: query.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return player == null
            ? null
            : new PlayerInfo(
            player.ResellerId,
            player.ResellerCode,
            player.LicenseeId,
            player.LicenseeCode,
            player.BrandId,
            player.BrandCode,
            player.PlayerCode,
            player.Username,
            player.DefaultWalletId,
            player.DefaultWalletCurrencyCode);
    }
}
