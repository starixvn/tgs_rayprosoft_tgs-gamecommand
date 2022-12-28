using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    [HttpGet("{playerCode}", Name = "Player_GetPlayer")]
    public ValueTask<PlayerInfo?> GetAsncy(
        [FromServices] IGetService<PlayerQuery, PlayerInfo?> service,
        string playerCode)
        => service
        .GetAsync(
            query: new PlayerQuery(playerCode),
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{playerCode}/status", Name = "Player_GetStatus")]
    public ValueTask<PlayerStatus?> GetStatusAsync(
        [FromServices] IPlayerRepository repository,
        string playerCode)
        => repository.GetStatusAsync(playerCode);
}
