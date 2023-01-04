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
}
