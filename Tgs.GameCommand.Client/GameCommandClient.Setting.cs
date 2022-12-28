using System.Net;
using System.Text.Json;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<TConfiguration?> SettingGetConfigurationAsync<TConfiguration>(
        string key,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/setting/{key}/value",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var value = response.StatusCode == HttpStatusCode.NoContent
            ? default
            : JsonSerializer.Deserialize<string>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false));

        return JsonSerializer.Deserialize<TConfiguration>(json: value ?? string.Empty);
    }

    public async ValueTask<string?> SettingGetConfigurationStringAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/setting/{key}/value",
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
}
