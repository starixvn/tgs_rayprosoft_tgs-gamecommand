using System.Net;
using System.Text.Json;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
	public async ValueTask<IEnumerable<GameActionInfo>?> GameActionQueryAsync(
		Guid roundId,
		CancellationToken cancellationToken = default)

	{
		var response = await _httpClient
			.GetAsync(
				requestUri: $"/gameaction/{roundId}",
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		return response.StatusCode == HttpStatusCode.NoContent
			? Enumerable.Empty<GameActionInfo>()
			: JsonSerializer.Deserialize<IEnumerable<GameActionInfo>>(
				json: await response
					.Content
					.ReadAsStringAsync(
						cancellationToken: cancellationToken)
					.ConfigureAwait(false),
				options: _SerializerOptions);
	}
}
