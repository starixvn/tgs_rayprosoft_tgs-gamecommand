using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[Route("[controller]")]
[ApiController]
public class GameActionController : ControllerBase
{
	[HttpGet("{id}", Name = "GameAction_Query")]
	public IAsyncEnumerable<GameActionInfo> QueryAsync(
		[FromServices] IGameActionRepository repository,
		Guid id)
		=> repository
		.QueryAsync(
			id: id,
			cancellationToken: HttpContext.RequestAborted);
}
