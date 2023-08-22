using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<Guid?> BrandGetBrandIdAsync(
        string brandCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/brand/{brandCode}/id",
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

    public async ValueTask<string?> BrandGetCurrencyAsync(
        Guid brandId,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/currency"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["currencyCode"] = currencyCode.ToString();
        uriBuilder.Query = query.ToString();
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
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

    public async ValueTask<BrandGameProviderInfo?> BrandGetGameProviderConfigurationAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/gp/configuration"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["gameProviderId"] = gameProviderId.ToString();
        uriBuilder.Query = query.ToString();
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<BrandGameProviderInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async IAsyncEnumerable<BrandGameProviderInfo> BrandQueryGameProviderAsync(
        Guid brandId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/gp/query"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        uriBuilder.Query = query.ToString();

        var response = await _httpClient
            .GetAsync(
            requestUri: uriBuilder.Uri,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var infos = JsonSerializer.Deserialize<IEnumerable<BrandGameProviderInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<BrandGameProviderInfo>();

        if (!infos.Any())
        {
            yield break;
        }

        foreach (var info in infos)
        {
            yield return info;
        }
    }

    public async ValueTask<Guid?> BrandGetGameProviderConfigurationIdAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/gp/configuration/id"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["gameProviderId"] = gameProviderId.ToString();
        uriBuilder.Query = query.ToString();
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<Guid?>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<GameProviderStatus?> BrandGetGameProviderStatusAsync(
        Guid brandId,
        Guid gameProviderId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/gp/status"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["gameProviderId"] = gameProviderId.ToString();
        uriBuilder.Query = query.ToString();
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
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

    public async ValueTask<GameStatus?> BrandGetGameStatusAsync(
        Guid brandId,
        int gameId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/game/status"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["gameId"] = gameId.ToString();
        uriBuilder.Query = query.ToString();
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameStatus>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<BrandStatus?> BrandGetStatusAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand/{brandId}/status"));
        var response = await _httpClient
            .GetAsync(
                requestUri: uriBuilder.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<BrandStatus>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async IAsyncEnumerable<BrandInfo> BrandQueryAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/brand"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        uriBuilder.Query = query.ToString();

        var response = await _httpClient
            .GetAsync(
            requestUri: uriBuilder.Uri,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var infos = JsonSerializer.Deserialize<IEnumerable<BrandInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<BrandInfo>();

        if (!infos.Any())
        {
            yield break;
        }

        foreach (var info in infos)
        {
            yield return info;
        }
    }
}
