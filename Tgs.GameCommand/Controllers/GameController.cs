using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    [HttpGet("{id}", Name = "Game_Get")]
    public ValueTask<GameInfo?> GetAsync(
        [FromServices] IGameRepository repository,
        int id)
        => repository.GetAsync(
            id: id,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{gameProviderCode}/{gameExternalId}", Name = "Game_GameProviderCode_ExternalGameId_Get")]
    public ValueTask<GameInfo?> GetAsync(
        [FromServices] IGetService<GameExternalQuery, GameInfo?> service,
        string gameProviderCode,
        string gameExternalId)
        => service.GetAsync(
            query: new GameExternalQuery(gameProviderCode, gameExternalId),
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{gameProviderid:guid}/{gameExternalId}", Name = "Game_GameProviderId_ExternalGameId_Get")]
    public ValueTask<GameInfo?> GetAsync(
        [FromServices] IGetService<GameGameProviderIdExternalQuery, GameInfo?> service,
        Guid gameProviderid,
        string gameExternalId)
        => service.GetAsync(
           query: new GameGameProviderIdExternalQuery(gameProviderid, gameExternalId),
           cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{gameProviderid:guid}/{gameExternalId}/id", Name = "Game_GameProviderId_ExternalGameId_Get_Id")]
    public ValueTask<int?> GetIdAsync(
        [FromServices] IGameRepository repository,
        Guid gameProviderid,
        string gameExternalId)
        => repository.GetIdAsync(
           gameProviderId: gameProviderid,
           gameExternalId: gameExternalId,
           cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{id}/status", Name = "Game_GetStatus")]
    public ValueTask<GameStatus> GetStatusAsync(
        [FromServices] IGameRepository repository,
        int id)
        => repository
        .GetStatusAsync(
            id,
            HttpContext.RequestAborted);

    [HttpGet("query/{gameProviderCode}", Name = "Game_Query")]
    public IAsyncEnumerable<GameInfo?> QueryAsync(
        [FromServices] IQueryService<GameByGameProviderCodeQuery, GameInfo> service,
        string gameProviderCode)
        => service
        .QueryAsync(
            query: new GameByGameProviderCodeQuery(gameProviderCode),
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("query/{gameProviderCode}/{type}", Name = "Game_QueryByGameType")]
    public IAsyncEnumerable<GameInfo?> QueryByGameTypeAsync(
        [FromServices] IQueryService<GameByGameTypeQuery, GameInfo> service,
        string gameProviderCode,
        GameType type)
        => service
        .QueryAsync(
            query: new GameByGameTypeQuery(gameProviderCode, type),
            cancellationToken: HttpContext.RequestAborted);
}
