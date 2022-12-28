using System.Net;
using System.Text.Json;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<ResellerStatus?> ResellerGetStatusAsync(
        Guid resellerId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/reseller/{resellerId}/status",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<ResellerStatus>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }
}
