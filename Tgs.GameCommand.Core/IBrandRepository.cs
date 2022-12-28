using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core;

public interface IBrandRepository
{
    ValueTask<Guid?> GetBrandIdAsync(
        string brandCode,
        CancellationToken cancellationToken = default);

    ValueTask<string?> GetCurrencyAsync(
        Guid brandId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<BrandGameProviderInfo?> GetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<Guid?> GetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<GameStatus?> GetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default);

    ValueTask<BrandStatus?> GetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default);
}
