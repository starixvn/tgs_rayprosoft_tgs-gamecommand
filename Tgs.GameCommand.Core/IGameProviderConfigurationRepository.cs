using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IGameProviderConfigurationRepository
{
    ValueTask<GameProviderConfigInfo?> GetAsync(
        Guid gameProviderConfigId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameProviderConfigInfo> QueryAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default);
}
