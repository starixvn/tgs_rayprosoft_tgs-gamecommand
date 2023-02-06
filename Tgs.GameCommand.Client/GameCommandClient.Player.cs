using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<PlayerInfo?> PlayerGetAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/player/{playerCode}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<PlayerInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async ValueTask<PlayerStatus?> PlayerGetStatusAsync(
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                 requestUri: $"/player/{playerCode}/status",
                 cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<PlayerStatus>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async IAsyncEnumerable<PlayerInfo> PlayerQueryAsync(
        Guid brandId,
        int pageIndex = 1,
        int pageSize = 1000,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        var queryStrings = new QueryBuilder();

        queryStrings.Add("pageIndex", pageIndex.ToString());
        queryStrings.Add("pageSize", pageSize.ToString());

        var response = await _httpClient
        .GetAsync(
                requestUri: $"/player/{brandId}{queryStrings.ToQueryString().Value}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var gameModels = JsonSerializer.Deserialize<IEnumerable<PlayerInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<PlayerInfo>();

        if (!gameModels.Any())
        {
            yield break;
        }

        foreach (var gameModel in gameModels)
        {
            yield return gameModel;
        }
    }
}
