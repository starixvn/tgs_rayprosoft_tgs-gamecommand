using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core.Models;

namespace Tgs.GameCommand.Core;

public interface IGameProviderRepository
{
    ValueTask<GameProviderInfo?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    ValueTask<string?> GetCultureCodeAsync(
        Guid gameProviderId,
        string cultureCode,
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderCurrencyModel?> GetCurrencyAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<string?> GetCurrencyCodeAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<Guid?> GetGameProviderIdAsync(
        string code,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameProviderIdInfo> GetGameProviderIdsAsync(
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderStatus> GetStatusAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
