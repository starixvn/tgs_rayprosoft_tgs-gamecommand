using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
	[HttpGet("{playerCode}", Name = "Player_GetPlayer")]
	public ValueTask<PlayerInfo?> GetAsncy(
		[FromServices] IPlayerRepository repository,
		string playerCode)
		=> repository
		.GetAsync(
			playerCode: playerCode,
			cancellationToken: HttpContext.RequestAborted);

	[HttpGet("{playerCode}/status", Name = "Player_GetStatus")]
	public ValueTask<PlayerStatus?> GetStatusAsync(
		[FromServices] IPlayerRepository repository,
		string playerCode)
		=> repository.GetStatusAsync(playerCode);

	[HttpGet("{brandId:guid}", Name = "Player_QueryA")]
	public IAsyncEnumerable<PlayerInfo> QueryAsync(
		[FromServices] IPlayerRepository repository,
		Guid brandId,
		int pageIndex = 1,
		int pageSize = 1000)
		=> repository
		.QueryAsync(
			brandId: brandId,
			pageIndex: pageIndex,
			pageSize: pageSize,
			cancellationToken: HttpContext.RequestAborted);
}
