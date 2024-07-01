using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.GameCommand.Client;

internal partial class GameCommandClient : IGameCommandClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	private readonly HttpClient _httpClient;

	public GameCommandClient(HttpClient httpClient)
		=> _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
}
