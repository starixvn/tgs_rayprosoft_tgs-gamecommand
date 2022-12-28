using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<GameProviderInfo?> GameProviderGetAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderCode}/code",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameProviderInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<GameProviderInfo?> GameProviderGetAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderId}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameProviderInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<string?> GameProviderGetCultureCodeAsync(
        Guid gameProviderId,
        string cultureCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                 requestUri: $"/gameprovider/{gameProviderId}/culturecode?cultureCode={cultureCode}",
                 cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<string>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<GameProviderCurrencyInfo?> GameProviderGetCurrencyAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderId}/currency?currencyCode={currencyCode}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameProviderCurrencyInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<string?> GameProviderGetCurrencyCodeAsync(
        Guid gameProviderId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderId}/currencycode?currencyCode={currencyCode}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<string>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<Guid?> GameProviderGetGameProviderIdAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderCode}/id",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<Guid>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async IAsyncEnumerable<GameProviderIdInfo> GameProviderGetGameProviderIdsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
        .GetAsync(
                requestUri: $"/gameprovider/query/id",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var gameModels = JsonSerializer.Deserialize<IEnumerable<GameProviderIdInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<GameProviderIdInfo>();

        if (!gameModels.Any())
        {
            yield break;
        }

        foreach (var gameModel in gameModels)
        {
            yield return gameModel;
        }
    }

    public async ValueTask<GameProviderStatus?> GameProviderGetStatusAsync(
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameprovider/{gameProviderId}/status",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameProviderStatus>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }
}
