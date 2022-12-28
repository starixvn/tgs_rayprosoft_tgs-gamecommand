using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
    public async ValueTask<GameProviderConfigInfo?> GameProviderConfigurationGetAsync(
        Guid gameProviderConfigId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameproviderconfiguration/{gameProviderConfigId}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : JsonSerializer.Deserialize<GameProviderConfigInfo>(
                json: await response
                    .Content
                    .ReadAsStringAsync(
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false),
                options: _SerializerOptions);
    }

    public async IAsyncEnumerable<GameProviderConfigInfo> GameProviderConfigurationQueryAsync(
        string gameProviderCode,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(
                requestUri: $"/gameproviderconfiguration/query/{gameProviderCode}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var gameProviderConfigs = JsonSerializer.Deserialize<IEnumerable<GameProviderConfigInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<GameProviderConfigInfo>();

        if (!gameProviderConfigs.Any())
        {
            yield break;
        }

        foreach (var gameProviderConfig in gameProviderConfigs)
        {
            yield return gameProviderConfig;
        }
    }
}
