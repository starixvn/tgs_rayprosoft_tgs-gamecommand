using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
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

    public async IAsyncEnumerable<ResellerInfo> ResellerQueryAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/reseller"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        uriBuilder.Query = query.ToString();

        var response = await _httpClient
            .GetAsync(
            requestUri: uriBuilder.Uri,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var infos = JsonSerializer.Deserialize<IEnumerable<ResellerInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<ResellerInfo>();

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
