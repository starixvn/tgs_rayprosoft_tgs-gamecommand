using System.Net;
using System.Text.Json;
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
}
