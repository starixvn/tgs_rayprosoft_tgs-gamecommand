using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

public interface IGameCommandClient
{
    ValueTask<Guid?> BrandGetBrandIdAsync(
        string brandcode,
        CancellationToken cancellationToken = default);

    ValueTask<string?> BrandGetCurrencyAsync(
        Guid brandId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<BrandGameProviderInfo?> BrandGetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<Guid?> BrandGetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderStatus?> BrandGetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<GameStatus?> BrandGetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default);

    ValueTask<BrandStatus?> BrandGetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<GameActionInfo>?> GameActionQueryAsync(
        Guid roundId,
        CancellationToken cancellationToken = default);

    ValueTask<GameInfo?> GameGetAsync(
        int gameId,
        CancellationToken cancellationToken = default);

    ValueTask<GameInfo?> GameGetAsync(
       string gameProviderCode,
       string gameExternalId,
       CancellationToken cancellationToken = default);

    ValueTask<GameInfo?> GameGetAsync(
       Guid gameProviderId,
       string gameExternalId,
       CancellationToken cancellationToken = default);

    ValueTask<int?> GameGetIdAsync(
       Guid gameProviderId,
       string gameExternalId,
       CancellationToken cancellationToken = default);

    ValueTask<GameStatus?> GameGetStatusAsync(
       int gameid,
       CancellationToken cancellationToken = default);

    ValueTask<GameProviderConfigInfo?> GameProviderConfigurationGetAsync(
        Guid gameProviderConfigId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameProviderConfigInfo> GameProviderConfigurationQueryAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderInfo?> GameProviderGetAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    ValueTask<string?> GameProviderGetCultureCodeAsync(
        Guid gameProviderId,
        string cultureCode,
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderCurrencyInfo?> GameProviderGetCurrencyAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<string?> GameProviderGetCurrencyCodeAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<Guid?> GameProviderGetGameProviderIdAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameProviderIdInfo> GameProviderGetGameProviderIdsAsync(
        CancellationToken cancellationToken = default);

    ValueTask<GameProviderStatus?> GameProviderGetStatusAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameInfo> GameQueryAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameInfo> GameQueryAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameInfo> GameQueryAsync(
        string gameProviderCode,
        GameType gameType,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameInfo> GameQueryAsync(
        Guid gameProviderId,
        GameType gameType,
        CancellationToken cancellationToken = default);

    ValueTask<LicenseeStatus?> LicenseeGetStatusAsync(
        Guid licenseeId,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerInfo?> PlayerGetAsync(
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerStatus?> PlayerGetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<ResellerStatus?> ResellerGetStatusAsync(
        Guid resellerId,
        CancellationToken cancellationToken = default);

    ValueTask<TConfiguration?> SettingGetConfigurationAsync<TConfiguration>(
        string key,
        CancellationToken cancellationToken = default);

    ValueTask<string?> SettingGetConfigurationStringAsync(
       string key,
       CancellationToken cancellationToken = default);
}
