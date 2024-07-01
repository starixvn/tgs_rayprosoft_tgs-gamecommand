using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[Route("[controller]")]
[ApiController]
public class GameProviderConfigurationController : ControllerBase
{
	[HttpGet("{gameProviderConfigId}", Name = "GameProviderConfiguration_Get")]
	public ValueTask<GameProviderConfigInfo?> GetAsync(
	   [FromServices] IGameProviderConfigurationRepository repository,
	   Guid gameProviderConfigId)
		=> repository
		.GetAsync(
			gameProviderConfigId: gameProviderConfigId,
			cancellationToken: HttpContext.RequestAborted);

	[HttpGet("query/{gameProviderCode}", Name = "GameProviderConfiguration_Query")]
	public IAsyncEnumerable<GameProviderConfigInfo> QueryAsync(
		[FromServices] IGameProviderConfigurationRepository repository,
		string gameProviderCode)
		=> repository
		.QueryAsync(
			gameProviderCode: gameProviderCode,
			cancellationToken: HttpContext.RequestAborted
			);
}
