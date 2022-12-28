using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameProviderGetService : IGetService<GameProviderQuery, GameProviderInfo?>
{
    private readonly IGameProviderRepository _repository;

    public GameProviderGetService(IGameProviderRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask<GameProviderInfo?> GetAsync(
        GameProviderQuery query,
        CancellationToken cancellationToken = default)
    {
        var model = await _repository
            .GetAsync(
                id: query.Id,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (model is null)
        {
            return null;
        }

        var status = await _repository
            .GetStatusAsync(
                id: model.GameProviderId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameProviderInfo(
            GameProviderId: model.GameProviderId,
            Description: model.Description,
            AutoCloseRound: model.AutoCloseRound,
            SupportTestPlayer: model.SupportTestPlayer,
            AllowKickPlayer: model.AllowKickPlayer,
            PlaceBetTimeout: model.PlaceBetTimeout,
            SettleBetTimeout: model.SettleBetTimeout,
            UniqueId: model.UniqueId,
            TransferWallet: model.TransferWallet,
            GameProviderCode: model.GameProviderCode,
            Name: model.Name,
            Localization: model.Localization);
    }
}
