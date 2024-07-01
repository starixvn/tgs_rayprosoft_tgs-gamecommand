using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient
{
	public async ValueTask<LicenseeStatus?> LicenseeGetStatusAsync(
		Guid licenseeId,
		CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync(
				requestUri: $"/licensee/{licenseeId}/status",
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		return response.StatusCode == HttpStatusCode.NoContent
			? null
			: JsonSerializer.Deserialize<LicenseeStatus>(
				json: await response
					.Content
					.ReadAsStringAsync(
						cancellationToken: cancellationToken)
					.ConfigureAwait(false),
				options: _SerializerOptions);
	}

	public async IAsyncEnumerable<LicenseeInfo> LicenseeQueryAsync(
	   [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"/licensee"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		uriBuilder.Query = query.ToString();

		var response = await _httpClient
			.GetAsync(
			requestUri: uriBuilder.Uri,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var contentJson = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

		var infos = JsonSerializer.Deserialize<IEnumerable<LicenseeInfo>>(contentJson, _SerializerOptions) ?? Enumerable.Empty<LicenseeInfo>();

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
