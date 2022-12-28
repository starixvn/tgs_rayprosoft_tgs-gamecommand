using Tgs.GameCommand.Common;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Core.ApplicationServices;

internal class GameProviderStatusGetService : IGetService<GameProviderStatusQuery, GameProviderStatus?>
{
    private readonly IGameProviderRepository _repository;

    public GameProviderStatusGetService(IGameProviderRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async ValueTask<GameProviderStatus?> GetAsync(GameProviderStatusQuery query, CancellationToken cancellationToken = default)
    {
        var id = await _repository
            .GetGameProviderIdAsync(
                code: query.Code,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        return !id.HasValue
            ? null
            : await _repository
            .GetStatusAsync(
                id: id.Value,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
